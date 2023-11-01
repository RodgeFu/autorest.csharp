import { MessageItem } from "../../Utils/messageItem";
import { ONE_INDENT, strcmp } from "../../Utils/utils";
import { AiPayloadApplyOutput } from "../Ai/FunctionParameter/AiPayloadApplyOutput";
import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { ExampleDesc } from "../Example/ExampleDesc";
import { ExampleValueDesc } from "../Example/ExampleValueDesc";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldFactory, ParamFieldType } from "./ParamFieldFactory";

type paramPos = { pos: "ctor" | "extraProperty", index: number };

export class ParamFieldObject extends ParamFieldBase {

    private normalizeNameForAiCompare(name: string | undefined): string | undefined {
        if (name === undefined)
            return undefined;
        let arr = [...name.matchAll(/^disable(.+)$/gi)];
        if (arr.length === 1) {
            name = `is${arr[0][1]}disabled`;
        }
        arr = [...name.matchAll(/^enable(.+)$/gi)];
        if (arr.length === 1) {
            name = `is${arr[0][1]}enabled`;
        }
        return name.toLowerCase();
    }

    private comparePropertyNameConsiderAI(propA: string, propB: string): boolean {

        let aName = this.normalizeNameForAiCompare(propA);
        let bName = this.normalizeNameForAiCompare(propB);
        return aName === bName;
    }

    protected override applyAiPayloadInternal(payload: any, output: AiPayloadApplyOutput) {

        let list = this.getDefaultValueWhenNotNull() as ParamFieldBase[];
        list.forEach(v => { if (v.isIgnorable) v.isIgnore = true; });
        let payloadKeys = Object.keys(payload);
        let allFieldIgnored = true;
        let listToDoubleCheck: ParamFieldBase[] = [];

        list.forEach(field => {
            // case insensitive considering AI...
            let foundKeyIndex = payloadKeys.findIndex(k => this.comparePropertyNameConsiderAI(k, field.fieldName));
            if (foundKeyIndex >= 0) {
                let foundKey = payloadKeys[foundKeyIndex];
                let foundValue = payload[foundKey];

                if (foundValue !== undefined) {
                    field.applyAiPayload(foundValue, output);
                    allFieldIgnored = false;
                }

                payloadKeys.splice(foundKeyIndex, 1);
            }
            else {
                listToDoubleCheck.push(field);
            }
        })

        listToDoubleCheck.forEach(field => {
            // consider flatten
            let path = field.serializerPath;
            if (path && path.length > 0) {
                let arr = path.split("/");
                if (arr.length > 1) {
                    let foundKeyIndex = payloadKeys.findIndex(k => this.comparePropertyNameConsiderAI(k, arr[0]));
                    if (foundKeyIndex >= 0) {
                        let curPayload = payload;
                        for (let seg of arr) {
                            let curKey = Object.keys(curPayload).find(k => this.comparePropertyNameConsiderAI(k, seg));
                            if (curKey !== undefined)
                                curPayload = curPayload[seg];
                            else
                                return;
                        }
                        field.applyAiPayload(curPayload, output);
                        allFieldIgnored = false;
                        payloadKeys.splice(foundKeyIndex, 1);
                    }
                }
            }
        })

        //var keysNotFound: string[] = [];
        //var allFieldIgnored = true;
        //Object.keys(payload).forEach(key => {
        //    let foundValue = payload[key];
        //    let foundField = list.find(v => v.fieldName === key);
        //    if (foundValue !== undefined && foundField !== undefined) {
        //        foundField.applyAiPayload(foundValue, output);
        //        allFieldIgnored = false;
        //    }
        //    else {
        //        // sometimes flattened payload will be returned in unflattend format
        //        list.find(v => {
        //            let arr = v.serializerPath.split("/");
        //            if (arr.length > 1) {

        //                for (let seg of arr) {
        //                    if (seg !== key)
        //                        return false;
        //                    else

        //                }
        //            }
        //        })


        //        keysNotFound.push(key);
        //        output.needDoubleCheck.push(`${this.fieldName}.${key}=${anyToString(payload[key])}`)
        //    }
        //})

        this.valueAsProperties = list;
        if (payloadKeys.length > 0)
            this.lastMessage = new MessageItem("Unabled to apply following properties in payload: " + payloadKeys.join(","), "warning");
        else if (allFieldIgnored)
            this.lastMessage = new MessageItem("All properties are marked as Ignored", "warning");
    }

    public override generateAiPayloadInternal(): any {
        const dict: { [index: string]: any } = {};
        this.getChildren().filter(c => !c.isIgnore).forEach(c => {
            dict[c.fieldName] = c.generateAiPayload();
        });
        return dict;
    }

    public override setExampleValue(value: ExampleValueDesc): void {
        if (value.propertyValues === undefined)
            this.valueAsProperties = undefined;
        else if (value.propertyValuesMap === undefined || value.propertyValuesMap.size === 0)
            this.valueAsProperties = this.getDefaultValueWhenNotNull();
        else {
            let list = this.getDefaultValueWhenNotNull() as ParamFieldBase[];
            list.forEach(v => { if (v.isIgnorable) v.isIgnore = true; });
            value.propertyValuesMap.forEach(p => {
                let foundExampleValue: ExampleValueDesc | undefined = undefined;

                let found = list.find(v => {
                    foundExampleValue = v.findMatchExample(p);
                    if (foundExampleValue)
                        return true;
                    else
                        return false;
                });
                if (found && foundExampleValue)
                    found.setExampleValue(foundExampleValue);
            })
            this.valueAsProperties = list;
        }
    }

    public override setValueInGenerateExampleValue(exampleDesc: ExampleDesc, exampleValue: ExampleValueDesc): void {
        if (this.valueAsProperties) {
            exampleValue.propertyValuesMap = new Map<string, ExampleValueDesc>();
            this.valueAsProperties?.forEach(item => {
                const ev = item.generateExampleValue(exampleDesc);
                if (ev)
                    exampleValue.propertyValuesMap?.set(item.fieldName, ev);
            });
        }
        else {
            exampleValue.propertyValuesMap = undefined;
        }
    }

    public override linkRelatedExamples(examples: ExampleValueDesc[]): void {
        super.linkRelatedExamples(examples);

        this.linkRelatedExamplesToObjectProperties(examples);
    }

    protected linkRelatedExamplesToPropertyList(propList: ParamFieldBase[], exampleList: ExampleValueDesc[]) {
        propList.forEach(prop => {
            let arr: ExampleValueDesc[] = [];
            exampleList.forEach(ex => {
                ex.propertyValuesMap?.forEach(propEx => {
                    let foundMatchExampleValue = prop.findMatchExample(propEx);
                    if (foundMatchExampleValue)
                        arr.push(foundMatchExampleValue);
                })
            });
            prop.linkRelatedExamples(arr);
        })
    }

    protected linkRelatedExamplesToObjectProperties(examples: ExampleValueDesc[]): void {
        if (this.valueAsProperties)
            this.linkRelatedExamplesToPropertyList(this.valueAsProperties, examples);
    }

    public override getChildren(): ParamFieldBase[] {
        let r: ParamFieldBase[] = [];
        this.valueAsProperties?.forEach(v => {
            r.push(v);
        })
        return r;
    }

    public get valueAsProperties(): ParamFieldBase[] | undefined {
        return this.value;
    }
    protected set valueAsProperties(list: ParamFieldBase[] | undefined) {
        this.value = list;
    }

    public set valueAsString(str: string | undefined) {
        if (str === undefined || str.length === 0 || str === "default")
            this.value = undefined;
        else
            throw Error("set object through string not supported");
    }

    public get valueAsString(): string | undefined {
        return this.implType.fullNameWithNamespace;
    }

    protected getValueForCodeInternal(indent: string, formatter: CodeFormatter): string {
        if (!this.implType.schemaObject?.defaultCreateMethod) {
            console.error("No method found to create the object: " + this.implType.fullNameWithNamespace);
            return "";
        }

        let methodName = this.implType.getFullNameWithNamespace(true /*includ global::*/, false /*include ending ?*/);
        if (this.implType.schemaObject.isDefaultCreateMethodStatic) {
            methodName += "." + this.implType.schemaObject.defaultCreateMethod.name;
        }
        else {
            methodName = "new " + methodName;
        }
        if (this.valueAsProperties === undefined)
            return "null";
        else if (this.valueAsProperties.length == 0)
            return `${methodName}()`;
        else {

            let ctorParams: ParamFieldBase[] = [];
            let hasComplexCtorParam: boolean = false;
            let extraProperties: ParamFieldBase[] = [];
            this.valueAsProperties.forEach((v, k) => {
                let tag: paramPos = v.tag;
                if (tag.pos === "ctor") {
                    ctorParams.push(v);
                    if (v.getChildren().length > 0)
                        hasComplexCtorParam = true;
                }
                else if (tag.pos == "extraProperty") {
                    extraProperties.push(v);
                }
                else {
                    console.error("unknown paramPos: " + tag.pos);
                }
            });

            let r = "";
            if (ctorParams.length === 1) {
                r = `${methodName}(${ctorParams[0].getValueForCode(indent, formatter)})`;
            }
            else if (ctorParams.length <= 3 && !hasComplexCtorParam) {
                let s = ctorParams.map(v => `${v.fieldName}: ${v.getValueForCode(indent, formatter)}`).join(", ");
                r = `${methodName}(${s})`;
            }
            else {
                let s = ctorParams.map(v => `${indent + ONE_INDENT}${v.fieldName}: ${v.getValueForCode(indent + ONE_INDENT, formatter)}`).join(",\n");
                r = `${methodName}(\n${s})`;
            }

            let filteredExtra = extraProperties.filter(v => !v.isIgnore);
            if (filteredExtra.length > 0) {
                let s = filteredExtra.map(v => `${indent + ONE_INDENT}${v.fieldName} = ${v.getValueForCode(indent + ONE_INDENT, formatter)}`).join(",\n");
                r += `\n${indent}{\n` + s + `\n${indent}}`;
            }
            return r;
        }
    }

    public get implType(): TypeDesc {
        return this.type;
    }

    public override getDefaultValueWhenNotNull(): any {
        let list = this.generatePropertyParamFieldList(this.implType);
        return list;
    }

    public override resetToSampleDefault() {
        let parent = this.parent;
        while (parent !== undefined) {
            if (parent.type.schemaKey === this.type.schemaKey) {
                this.value = this.defaultValue;
                return;
            }
            parent = parent.parent;
        }
        this.value = this.getDefaultValueWhenNotNull();
        this.valueAsProperties?.forEach(p => p.resetToSampleDefault());
    }

    protected generatePropertyParamFieldList(tdw: TypeDesc) {
        let list: ParamFieldBase[] = [];
        let ctorList: ParamFieldBase[] = [];
        let propList: ParamFieldBase[] = [];

        if (tdw.schemaObject?.defaultCreateMethod) {
            let i = 0;
            tdw.schemaObject.defaultCreateMethod.methodParameters?.forEach(p => {
                let pf = ParamFieldFactory.createParamField(p.name!, p.type!, this,
                    { isReadonly: false, isRequired: !p.isOptional, parameterOwner: this.parameterOwner, serializerPath: p.relatedPropertySerializerPath ?? p.name!, idPrefix: this.idPrefix });
                pf.tag = { pos: "ctor", index: i++ } as paramPos;
                pf.description = p.description ?? "";
                if (p.defaultValue != undefined && p.defaultValue.length > 0) {
                    if (["object", "array", "dictionary"].includes(pf.fieldType)) {
                        console.error("not expected default value for object in serializationConstructor, array or dictoinary: " + p.name);
                    }
                    else {
                        pf.value = p.defaultValue;
                    }
                }
                ctorList.push(pf);
            });
            tdw.schemaObject.extraProperties?.forEach(p => {
                let pf = ParamFieldFactory.createParamField(p.name!, p.type!, this,
                    { isReadonly: p.isReadonly ?? false, isRequired: p.isRequired ?? false, parameterOwner: this.parameterOwner, serializerPath: p.serializerPath ?? p.name!, idPrefix: this.idPrefix });
                pf.tag = { pos: "extraProperty", index: i++ } as paramPos;
                pf.description = p.description ?? "";
                // seems the value will always be undefined because can't find anywhere to set it. comment for now and fix later if needed.
                //if (p.value != undefined && p.value.length > 0) {
                //  if (["object", "array", "dictionary"].includes(pf.uiType)) {
                //    console.error("not expected default value for object in serializationConstructor, array or dictoinary: " + p.name);
                //  }
                //  else {
                //    pf.value = p.value;
                //  }
                //}
                propList.push(pf);
                propList = propList.sort((a, b) => strcmp(a.fieldName, b.fieldName));
            })
        }
        else {
            console.error("unexpected serializationCtor or InitializationCtor: " + tdw.fullNameWithNamespace);
        }
        list = ctorList.concat(propList);
        return list
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
    }
}
