import { MessageItem } from "../../Utils/messageItem";
import { AiPayloadApplyOutput } from "../Ai/FunctionParameter/AiPayloadApplyOutput";
import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { ExampleValueDesc } from "../Example/ExampleValueDesc";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldFactory, ParamFieldType } from "./ParamFieldFactory";

type DictionaryItem = { key: ParamFieldBase, value: ParamFieldBase };

export class ParamFieldDictionaryItem extends ParamFieldBase {

    public readonly keyType: TypeDesc;
    public readonly valueType: TypeDesc;

    protected override applyAiPayloadInternal(payload: any, output: AiPayloadApplyOutput) {

        if (payload["key"] === undefined || payload["key"] == null)
            this.lastMessage = new MessageItem("'key' is missing", "error");
        else {
            this.valueAsDictionaryItem.key.applyAiPayload(payload["key"], output);
            this.valueAsDictionaryItem.value.applyAiPayload(payload["value"], output);
        }
    }

    public override setExampleValue(value: ExampleValueDesc): void {

        // TODO: double check on the case that ResouceIdentifier is key
        this.valueAsDictionaryItem.key.setExampleValue(new ExampleValueDesc({
            rawValue: value.fieldName,
            schemaType: this.keyType.name,
            cSharpName: "key",
        }, value.ownerExample, "key"));
        this.valueAsDictionaryItem.value.setExampleValue(value);
    }

    public override linkRelatedExamples(examples: ExampleValueDesc[]): void {
        super.linkRelatedExamples(examples);

        let arrKey: ExampleValueDesc[] = [];
        let arrValue: ExampleValueDesc[] = [];

        examples.forEach(ex => {
            arrKey.push(new ExampleValueDesc({
                rawValue: ex.fieldName,
                schemaType: this.keyType.name,
                serializerName: "key",
                cSharpName: "key",
                modelName: "key"
            }, ex.ownerExample, "key"));
            arrValue.push(ex);
        })
        this.valueAsDictionaryItem.key.linkRelatedExamples(arrKey);
        this.valueAsDictionaryItem.value.linkRelatedExamples(arrValue);
    }

    public get valueAsDictionaryItem(): DictionaryItem {
        return this.value;
    }

    public set valueAsDictionaryItem(v: DictionaryItem) {
        this.value = v;
    }

    public override getChildren(): ParamFieldBase[] {
        return this.value ? [this.valueAsDictionaryItem.key, this.valueAsDictionaryItem.value] : [];
    }

    public set valueAsString(str: string | undefined) {
        if (str === undefined || str.length === 0 || str === "default")
            this.value = undefined;
        else
            throw Error("set dictionary item through string not supported");
    }

    public get valueAsString(): string | undefined {
        return this.type.fullNameWithNamespace;
    }

    protected getValueForCodeInternal(indent: string, formatter: CodeFormatter): string {
        if (this.valueAsDictionaryItem.key === undefined || this.valueAsDictionaryItem.key.value === undefined)
            throw Error("key of dictionary is undefined for CodeEditor");

        return `[${this.valueAsDictionaryItem.key.getValueForCode(indent, formatter)}] = ${this.valueAsDictionaryItem.value.getValueForCode(indent, formatter)}`
    }

    public override getDefaultValueWhenNotNull(): any {
        if (!this.type.arguments || this.type.arguments.length !== 2) {
            throw Error("unexpected arguments for dict item. type: " + this.type.fullNameWithNamespace);
        }
        return {
            // be careful that this will be called in super() before this.ctor
            key: ParamFieldFactory.createParamField("key", this.type.arguments[0], this,
                { isReadonly: false, isRequired: true, serializerPath: "key", parameterOwner: this.parameterOwner, idPrefix: this.idPrefix }),
            value: ParamFieldFactory.createParamField("value", this.type.arguments[1], this,
                { isReadonly: false, isRequired: true, serializerPath: "value", parameterOwner: this.parameterOwner, idPrefix: this.idPrefix })
        };
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
        if (!this.type.arguments || this.type.arguments.length !== 2) {
            throw Error("unexpected arguments for dict item. type: " + this.type.fullNameWithNamespace);
        }
        else {
            this.keyType = this.type.arguments[0];
            this.valueType = this.type.arguments[1];
        }
    }
}
