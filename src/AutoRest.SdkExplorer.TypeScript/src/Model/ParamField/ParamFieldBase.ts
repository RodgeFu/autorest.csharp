import { isObject } from "underscore";
import { EventSlim } from "../../Utils/EventSlim";
import { logError } from "../../Utils/logger";
import { MessageItem } from "../../Utils/messageItem";
import { isStringEqualCaseInsensitive, isStringNullOrEmpty } from "../../Utils/utils";
import { AiFunctionDefinition } from "../Ai/AiFunctionDefinition";
import { AiDefaultParamDesc } from "../Ai/FunctionParameter/AiDefaultParamDesc";
import { AiEnumParamDesc } from "../Ai/FunctionParameter/AiEnumParamDesc";
import { AiObjectParamDesc } from "../Ai/FunctionParameter/AiObjectParamDesc";
import { AiParamDesc } from "../Ai/FunctionParameter/AiParamDesc";
import { AiPayloadApplyOutput } from "../Ai/FunctionParameter/AiPayloadApplyOutput";
import { ParameterDesc } from "../Code/ParameterDesc";
import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { NamespaceManager } from "../CodeGen/NamespaceManager";
import { ExampleDesc } from "../Example/ExampleDesc";
import { ExampleValueDesc } from "../Example/ExampleValueDesc";
import { ParamFieldType } from "./ParamFieldFactory";

export interface ValidationResult {
    pass: boolean;
    valueToValidate: any;
    message: MessageItem;
}

export interface ParamFieldExtraConstructorParameters {
    isReadonly: boolean,
    isRequired: boolean,
    serializerPath: string,
    parameterOwner: ParameterDesc,
    idPrefix: string,
    description?: string
};

export abstract class ParamFieldBase {

    // add this field so that we can keep the original value when setting to null which can be used when unsetting null
    private _isNull: boolean = false;
    public get isNull(): boolean {
        return this._isNull;
    }
    public set isNull(v: boolean) {
        if (v === true) {
            if (this.type.isNullable === false) {
                logError("value is null when the type is not nullable, reset to previous or default value. Type: " + this.type.fullNameWithNamespace);
                this._isNull = false;
                // keep the previous value if there is
                if (this.valueInternal === undefined)
                    this.valueInternal = this.getDefaultValueWhenNotNull();
            }
            else
                this._isNull = true;
        }
        else {
            this._isNull = false;
            if (this.valueInternal === undefined)
                this.valueInternal = this.getDefaultValueWhenNotNull();
            this.refreshRelatedExamples();
        }
        this.isIgnore = false;
        this.onValueSet.trigger(this);
    }

    private _isIgnore: boolean = false;
    public get isIgnore(): boolean {
        return this._isIgnore;
    }
    public set isIgnore(v: boolean) {
        if (v === true) {
            if (this.isRequired === true) {
                logError("value is ignore when the filed is required");
                this._isIgnore = false;
            }
            else
                this._isIgnore = true;
        }
        else {
            this._isIgnore = false;
        }
    }

    public get isIgnorable(): boolean {
        return !this.isRequired;
    }

    private valueInternal: any;
    public set value(v: any) {
        if (v === undefined || v === null) {
            this.isIgnore = false;
            this.isNull = true;
        }
        else {
            this.isIgnore = false;
            this.isNull = false;
            this.valueInternal = v;
            this.refreshRelatedExamples();
        }
        this.onValueSet.trigger(this);
    }
    public get value(): any {
        if (this.isNull)
            return undefined;
        return this.valueInternal;
    }

    protected setValueInGenerateExampleValue(exampleDesc: ExampleDesc, exampleValue: ExampleValueDesc): void {
        exampleValue.rawValue = this.valueAsString;
    }

    public generateExampleValue(exampleDesc: ExampleDesc): ExampleValueDesc | undefined {
        if (this.isIgnore)
            return undefined;
        const r = new ExampleValueDesc({
            arrayValues: undefined,
            rawValue: undefined,
            propertyValues: undefined,
            serializerName: this.serializerPath,
            cSharpName: this.fieldName,
            modelName: this.fieldName,
            schemaType: this.type.schemaType,
            description: this.description
        }, exampleDesc, this.fieldName);
        this.setValueInGenerateExampleValue(exampleDesc, r);
        return r;
    }

    public applyAiPayload(payload: any, output: AiPayloadApplyOutput) {
        if (payload === undefined)
            this.isIgnore = true;
        else if (payload === null)
            this.isNull = true;
        else {
            this.isIgnore = false;
            this.applyAiPayloadInternal(payload, output);
        }
    }

    public get isResourceIdentifierField(): boolean {
        return this.type.name.toLowerCase() === "resourceidentifier";
    }

    protected applyAiPayloadInternal(payload: any, output: AiPayloadApplyOutput) {
        output.needMoreInput.push(...this.getPendingUserInputs(payload));
        if (!isObject(payload)) {
            this.valueAsString = payload.toString();
            if (this.lastMessage && (this.lastMessage.level === "error" || this.lastMessage.level === "warning"))
                output.needDoubleCheck.push(`${this.fieldName}=${payload}`)
        }
        else {
            output.needDoubleCheck.push(`${this.fieldName}=${payload}`)
            this.lastMessage = new MessageItem(`Unexpected value to set '${payload}'. non-object is expected but got '${typeof payload}'`, "error");
        }
    }

    public generateAiPayload(): any {
        if (this.isIgnore)
            return undefined;
        if (this.isNull)
            return null;
        return this.generateAiPayloadInternal();
    }

    public generateAiPayloadInternal(): any {
        return this.value;
    }

    public generateAiParamDesc(): AiParamDesc {
        if (this.type.schemaType === "OBJECT_SCHEMA") {

            if (this.type.schemaObject!.isEnum === false) {
                const dict: { [index: string]: AiParamDesc } = {};
                const required: string[] = [];
                this.getChildren().filter(c => !c.isIgnore).forEach(c => {
                    dict[c.fieldName] = c.generateAiParamDesc();
                    if (c.isRequired)
                        required.push(c.fieldName);
                });
                return new AiObjectParamDesc(
                    this.description,
                    dict,
                    required);
            }
            else {
                return new AiEnumParamDesc(
                    this.description,
                    this.type.schemaObject?.enumValues.map(v => v.value) ?? []);
            }
        }
        else if (this.type.schemaType === "ENUM_SCHEMA") {
            return new AiEnumParamDesc(
                this.description,
                this.type.schemaEnum?.values.map(v => v.value) ?? []);
        }
        else if (this.type.schemaType === "NONE_SCHEMA") {
            return this.generateKnownAiParamDesc() ?? new AiParamDesc(
                this.type.schemaKey,
                this.description);
        }
        else {
            logError("unknown SchemaType when generating ai param desc: " + this.type.fullNameWithNamespace);
            return new AiParamDesc(
                this.type.schemaKey,
                this.description);
        }
    }

    public generateKnownAiParamDesc(): AiParamDesc | undefined {
        switch (this.type.schemaKey) {
            case "Azure.WaitUntil":
                return new AiDefaultParamDesc(this.description, "default");
            case "System.Threading.CancellationToken":
                return new AiDefaultParamDesc(this.description, "default");
            default:
                return undefined;
        }
    }

    public setExampleValue(value: ExampleValueDesc): void {
        this.valueAsString = value.rawValue;
    }

    public relatedExamples: ExampleValueDesc[] = [];
    public linkRelatedExamples(examples: ExampleValueDesc[]): void {
        this.relatedExamples = examples;
    }
    protected refreshRelatedExamples(): void {
        // trigger linkRelatedExamples because sub-class may override to do extra work
        this.linkRelatedExamples(this.relatedExamples);
    }

    public resetToNotNullDefault(): void {
        this.value = this.getDefaultValueWhenNotNull();
    }

    public resetToSampleDefault(): void {
        this.value = this.getDefaultValueWhenNotNull();
    }

    public resetToDefault(): void {
        this.value = this.defaultValue;
    }

    public resetToIgnoreOrDefault(): boolean {
        if (!this.isRequired) {
            this.isIgnore = true;
            return true;
        }
        else {
            this.resetToDefault();
            return false;
        }
    }

    public abstract set valueAsString(v: string | undefined);
    public abstract get valueAsString(): string | undefined;

    protected abstract getValueForCodeInternal(indent: string, formatter: CodeFormatter): string;

    //public get uiTitleId() {
    //    return this.uiId + "___title";
    //}

    //public get uiNullButtonId() {
    //    return this.uiId + "___nullbutton";
    //}

    //public get uiAddButtonId() {
    //    return this.uiId + "___addbutton";
    //}

    //public get uiAreaId() {
    //    return this.uiId + "___area";
    //}

    //public get uiCodeId() {
    //    return this.uiId + "___code";
    //}

    //public get uiId() {
    //    return this.id;
    //}

    public get fieldPath(): string {
        if (this.parent)
            return `${this.parent.fieldPath}/${this.fieldName}`;
        else
            return this.fieldName;
    }

    public tag?: any;
    //** usually it's true, i.e. will be false for discriminator property */
    public visibleInCode: boolean = true;

    public getValueForCode(indent: string, formatter: CodeFormatter): string {
        let r = "";
        if (this.value === undefined)
            r = "null"
        else
            r = this.getValueForCodeInternal(indent, formatter);

        if (formatter && formatter.formatParamFieldCode) {
            r = formatter.formatParamFieldCode(r, this);
        }

        return r;
    }

    public get isNullable(): boolean {
        return this.type.isNullable === true && !this.isReadonly;
    }

    public get defaultValue(): any | undefined {
        if (this.isNullable)
            return undefined;
        else {
            let r = this.getDefaultValueWhenNotNull();
            if (r === undefined || r === null) {
                throw Error("null is return from getDefaultValueWhenNotNull");
            }
            return r;
        }
    }
    public abstract getDefaultValueWhenNotNull(): any;

    public lastMessage?: MessageItem;
    // TODO: we need a better validation part
    public validateValue(): ValidationResult {
        return {
            pass: true,
            valueToValidate: this.value,
            message: new MessageItem()
        }
    }

    protected extraNamespaces: string[] = []

    protected getMyNamespaces(): string[] {
        let r: string[] = [];
        this.extraNamespaces.forEach(ns => {
            r.push(ns);
        })
        r.push(this.type.namespace!);
        this.type.arguments?.forEach(a => {
            r.push(a.namespace!);
        })
        return r;
    }

    public getNamespaces(): string[] {
        let helper = new NamespaceManager()

        helper.pushNamespaceArray(this.getMyNamespaces());

        this.getChildren().forEach(pf => {
            helper.pushNamespaceArray(pf.getNamespaces());
        });
        return helper.namespaces;
    }


    public getChildren(): ParamFieldBase[] {
        return [];
    }

    // return false to skip foreach children
    public forEachInTree(func: (field: ParamFieldBase) => boolean) {
        if (func(this))
            this.getChildren().forEach((c, i) => c.forEachInTree(func));
    }

    public get distnaceToLeaf(): number {
        let r = 0;
        this.getChildren().forEach(c => {
            let rr = c.distnaceToLeaf + 1;
            if (rr > r)
                r = rr;
        })
        return r;
    }

    public getLeafCount(includeIgnored: boolean): number {
        if (!includeIgnored && this.isIgnore)
            return 0;
        const children = this.getChildren();
        if (children.length === 0)
            return includeIgnored || !this.isIgnore ? 1 : 0;
        else {
            let r = 0;
            this.getChildren().forEach(c => r += c.getLeafCount(includeIgnored));
            return r;
        }
    }

    public getIgnoredLeafCount(): number {
        if (this.isIgnore)
            return 1;
        const children = this.getChildren();
        if (children.length === 0)
            return this.isIgnore ? 1 : 0;
        else {
            let r = 0;
            this.getChildren().forEach(c => r += c.getIgnoredLeafCount());
            return r;
        }
    }

    public get distanceToRoot(): number {
        if (this.parent === undefined)
            return 0;
        return this.parent.distanceToRoot + 1;
    }

    public findMatchExample(exampleValue: ExampleValueDesc): ExampleValueDesc | undefined {
        // just ignore case to make thing simple. (there won't be two property only differ in case in our codegen code)
        if (this.fieldName.toLowerCase() === exampleValue.cSharpName?.toLowerCase())
            return exampleValue;

        // check flattened single property object
        if (!this.isFlattenedSinglePropertyObject())
            return undefined;
        let arr = this.serializerPath.split("/");
        let curParamFieldIndex = 0;
        let curExampleValue = exampleValue;

        for (curParamFieldIndex = 0; curParamFieldIndex < arr.length - 1; curParamFieldIndex++) {
            if (!isStringEqualCaseInsensitive(curExampleValue.cSharpName, arr[curParamFieldIndex]))
                return undefined;
            curExampleValue = exampleValue.propertyValuesMap?.values().next().value;
            if (!curExampleValue)
                return undefined;
        }

        if (isStringEqualCaseInsensitive(curExampleValue.cSharpName, arr[curParamFieldIndex]))
            return curExampleValue;
        else
            return undefined;
    }

    public isFlattenedSinglePropertyObject(): boolean {
        return this.serializerPath.indexOf("/") > 0;
    }

    public get isSubscriptionIdParamField(): boolean {
        return this.parent === undefined && this.parameterOwner.suggestedName?.toLowerCase() === "subscriptionid";
    }

    public get isResourceGroupParamField(): boolean {
        return this.parent === undefined && this.parameterOwner.suggestedName?.toLowerCase() === "resourcegroupname";
    }

    protected _id: string;
    public get id(): string {
        return this._id;
    }
    onValueSet: EventSlim<void> = new EventSlim<void>();


    readonly isReadonly: boolean;
    readonly isRequired: boolean;
    readonly serializerPath: string;
    readonly parameterOwner: ParameterDesc;
    readonly idPrefix: string;
    description: string;

    constructor(public fieldName: string, public readonly fieldType: ParamFieldType, public readonly type: TypeDesc, public readonly parent: ParamFieldBase | undefined,
        params: ParamFieldExtraConstructorParameters) {

        this.isReadonly = params.isReadonly;
        this.isRequired = params.isRequired;
        this.serializerPath = params.serializerPath;
        this.parameterOwner = params.parameterOwner;
        this.idPrefix = isStringNullOrEmpty(params.idPrefix) ? "no_id_prefix" : params.idPrefix;
        this.description = params.description ?? "";

        if (this.parent)
            this._id = `${this.parent?._id ?? ""}___${this.fieldName}`;
        else
            this._id = `ParamParamField___${this.idPrefix}___${this.parameterOwner.suggestedName}___${this.fieldName}`;
        this.value = this.defaultValue;

        if (this.fieldType === "unknown") {
            this.lastMessage = new MessageItem("Unknown Type: " + this.type.fullNameWithNamespace, "error");
        }
    }

    private static readonly REF_PREFIX = "ref::";

    public static isRef(str?: string): boolean {
        return str?.startsWith(ParamFieldBase.REF_PREFIX) ?? false;
    }

    public static fromRef(str?: string): string | undefined {
        if (str && this.isRef(str))
            return str.substring(ParamFieldBase.REF_PREFIX.length);
        else
            return undefined;
    }

    public static toRef(str: string): string {
        return `${ParamFieldBase.REF_PREFIX}${str}`;
    }

    protected getPendingUserInputs(fieldValue: any): string[] {
        let strValue: string = fieldValue.toString() ?? "";
        let m = strValue.match(/{[^{}\n]+?}/g);
        if (m && m.length > 0) {
            if (this.parent?.isResourceIdentifierField) {
                return [strValue];
            }
            return m.map(value => value);
        }
        else
            return [];
    }

    public static applyAiPayload(fields: ParamFieldBase[], payload: any, output: AiPayloadApplyOutput) {

        fields.forEach(f => {
            const foundKey = Object.keys(payload).find(key => key === f.fieldName);
            if (foundKey) {
                f.applyAiPayload(payload[foundKey], output);
                output.needMoreInput.push(...f.getPendingUserInputs(payload[foundKey]));
            }
            else {
                f.applyAiPayload(undefined, output);
                f.isIgnore = true;
            }
        })

        const dict: { [index: string]: any } = {};
        fields.forEach(p => dict[p.fieldName] = p.generateAiPayload());
        return dict;
    }

    public static generateAiPayload(fields: ParamFieldBase[]): { [index: string]: any } {
        const dict: { [index: string]: any } = {};
        fields.forEach(p => dict[p.fieldName] = p.generateAiPayload());
        return dict;
    }

    public static generateAiFunctionDefinition(functionName: string, functionDescription: string, fields: ParamFieldBase[]) {
        const dict: { [index: string]: AiParamDesc } = {};
        const required: string[] = [];
        fields.forEach(p => {
            dict[p.fieldName] = p.generateAiParamDesc();
            required.push(p.fieldName);
        });
        return AiFunctionDefinition.create(
            functionName,
            functionDescription,
            new AiObjectParamDesc("", dict, required)
        );
    }

    public static applyExample(example: ExampleDesc, fields: ParamFieldBase[], subscriptionId?: string, resourceGroupName?: string) {
        let s: Set<string> = new Set<string>();
        example.exampleValuesMap?.forEach((value, key) => {
            let foundExampleValue: ExampleValueDesc | undefined = undefined;
            let p = fields.find(pp => {
                foundExampleValue = pp.findMatchExample(value);
                // ignore subid and resource group name unless they are provided specificly, because
                // 1. the value in example can't be right and user has to reset them if they are changed
                if (foundExampleValue && pp.isSubscriptionIdParamField && subscriptionId)
                    foundExampleValue.rawValue = subscriptionId;
                if (foundExampleValue && pp.isResourceGroupParamField && resourceGroupName)
                    foundExampleValue.rawValue = resourceGroupName;
                    
                return foundExampleValue !== undefined;
            });
            if (p !== undefined && foundExampleValue) {
                if ((p.isSubscriptionIdParamField && subscriptionId) ||
                    (p.isResourceGroupParamField && resourceGroupName) ||
                    (!p.isSubscriptionIdParamField && !p.isResourceGroupParamField)) {
                    // not load example value for subscriptionId and resourceGroup because
                    // 1. it can't be right, 2. user has to reset them again if they have been set
                    s.add(p.fieldName!);
                    p.setExampleValue(foundExampleValue);
                }
            }
            else {
                console.warn("can't find field for example: " + key);
            }
        })

        fields.forEach(v => {
            if ((!v.isSubscriptionIdParamField) &&
                (!v.isResourceGroupParamField) &&
                (!s.has(v.fieldName!)))
                v.resetToIgnoreOrDefault();
        })
    }
}
