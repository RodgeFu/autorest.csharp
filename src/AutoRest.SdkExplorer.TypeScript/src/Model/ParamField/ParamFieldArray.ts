import { format } from "path";
import { logWarning } from "../../Utils/logger";
import { ONE_INDENT } from "../../Utils/utils";
import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { ExampleValueDesc } from "../Example/ExampleValueDesc";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldFactory, ParamFieldType } from "./ParamFieldFactory";
import { ExampleDesc } from "../Example/ExampleDesc";
import { AiArrayParamDesc } from "../Ai/FunctionParameter/AiArrayParamDesc";
import { AiParamDesc } from "../Ai/FunctionParameter/AiParamDesc";

export class ParamFieldArray extends ParamFieldBase {

    public override setExampleValue(value: ExampleValueDesc): void {
        if (value.arrayValues === undefined)
            this.valueAsArray = undefined;
        else if (value.arrayValues.length === 0)
            this.valueAsArray = [];
        else {
            let arr = value.arrayValues.map((v, i) => {
                let itemName = this.generateItemName(i);
                let r = ParamFieldFactory.createParamField(itemName, this.arrayType, this,
                    { isReadonly: false, isRequired: false, serializerPath: itemName, parameterOwner: this.parameterOwner, idPrefix: this.idPrefix });
                r.setExampleValue(v);
                return r;
            })
            this.valueAsArray = arr;
            //this.refreshRelatedExamples();
        }
    }

    public override resetToSampleDefault() {
        this.resetToNotNullDefault();
        const item = this.appendItem();
        item.resetToSampleDefault();
    }

    protected override setValueInGenerateExampleValue(exampleDesc: ExampleDesc, exampleValue: ExampleValueDesc): void {
        if (this.valueAsArray !== undefined) {
            const arr : ExampleValueDesc[] = [];
            this.valueAsArray.forEach(v => {
                const ev = v.generateExampleValue(exampleDesc);
                if(ev)
                    arr.push(ev);
            })
            exampleValue.arrayValues = arr;
        }
    }

    public getRelatedExampleForChildItem(): ExampleValueDesc[] {
        return this.relatedExamples.flatMap(e => e.arrayValues ?? []);
    }

    public override linkRelatedExamples(examples: ExampleValueDesc[]): void {
        super.linkRelatedExamples(examples);
        this.valueAsArray?.forEach(v => v.linkRelatedExamples(examples.flatMap(ex => ex.arrayValues ?? [])));
    }

    public override getChildren(): ParamFieldBase[] {
        return this.valueAsArray ?? [];
    }

    public arrayType: TypeDesc;

    public get valueAsArray(): ParamFieldBase[] | undefined {
        return this.value;
    }
    public set valueAsArray(arr: ParamFieldBase[] | undefined) {
        this.value = arr;
    }

    public set valueAsString(str: string | undefined) {
        if (str === undefined)
            this.valueAsArray = undefined;
        else {
            let arr = JSON.parse(str);
            if (Array.isArray(arr)) {
                this.value = arr.map((v, index) => {
                    // TODO: potential perf issue because value will be convert to string and back. consider refactor when needed.
                    let name = this.generateItemName(index);
                    let r = ParamFieldFactory.createParamField(name, this.arrayType, this, { isReadonly: false, isRequired: false, serializerPath: name, parameterOwner: this.parameterOwner, idPrefix: this.idPrefix });
                    r.valueAsString = JSON.stringify(v);
                });
                //this.refreshRelatedExamples();
            }
            else {
                console.error("Array expacted for ParamParamFieldArray: " + str);
                this.valueAsArray = [];
            }
        }
    }

    public get valueAsString(): string | undefined {
        if (this.valueAsArray === undefined)
            return undefined;
        else {
            return `Array ${this.fieldName} [${this.valueAsArray.length}]`;
        }
    }

    protected getValueForCodeInternal(indent: string, formatter: CodeFormatter): string {
        if (this.valueAsArray === undefined)
            return "null";
        else if (this.valueAsArray.length == 0)
            return this.isReadonly ? '{}' : `global::System.Array.Empty<${this.arrayType.getFullNameWithNamespace(true /*includ global::*/, true /*include ending ?*/)}>()`;
        else {
            let s = "";
            this.valueAsArray?.forEach(w => s += indent + ONE_INDENT + w.getValueForCode(indent + ONE_INDENT, formatter) + ',\n');

            return (this.isReadonly ? '\n' : `new ${this.arrayType.getFullNameWithNamespace(true /*includ global::*/, false /*include ending ?*/)}[]\n`) +
                `${indent}{\n` +
                s +
                `${indent}}`;
        }
    }


    public override getDefaultValueWhenNotNull(): any {
        return [];
    }

    public appendItem(): ParamFieldBase {
        if (this.valueAsArray === undefined)
            this.valueAsArray = [];
        let name = this.generateItemName(this.valueAsArray.length);
        let item = ParamFieldFactory.createParamField(name, this.arrayType, this, { isReadonly: false, isRequired: true, serializerPath: name, parameterOwner: this.parameterOwner, idPrefix: this.idPrefix });
        this.valueAsArray.push(item);
        this.refreshRelatedExamples();
        return item;
    }

    public removeItem(fieldName: string) {
        if (!this.valueAsArray || this.valueAsArray.length === 0)
            return;

        var newArray: ParamFieldBase[] = [];
        let newIndex = 0;
        this.valueAsArray?.forEach((v, index) => {
            if (v.fieldName !== fieldName) {
                v.fieldName = this.generateItemName(newIndex++);
                newArray.push(v);
            }
        })
        this.valueAsArray = newArray;
    }

    private generateItemName(index: number) {
        return `Array${index}`;
    }

    protected override applyAiPayloadInternal(payload: any) {
        if (!Array.isArray(payload)) {
            console.error("Unexpected payload for array, ignore it. :\n" + payload);
            this.valueAsArray = [];
        }
        else {
            if (payload.length === 0)
                this.valueAsArray = [];
            else {
                let arr = payload.map((v, i) => {
                    let itemName = this.generateItemName(i);
                    let r = ParamFieldFactory.createParamField(itemName, this.arrayType, this,
                        { isReadonly: false, isRequired: false, serializerPath: itemName, parameterOwner: this.parameterOwner, idPrefix: this.idPrefix });
                    r.applyAiPayload(v);
                    return r;
                })
                this.valueAsArray = arr;
            }
        }
    }

    public override generateAiPayloadInternal(): any {
        if (this.valueAsArray && this.valueAsArray.length > 0)
            return this.valueAsArray.map(v => v.generateAiPayload());
        else
            return [];
    }

    public override generateAiParamDesc() {
        if (this.valueAsArray && this.valueAsArray.length > 0)
            return new AiArrayParamDesc(
                this.description,
                this.valueAsArray[0].generateAiParamDesc());
        else
            return new AiArrayParamDesc(
                this.description,
                new AiParamDesc(
                    "object",
                    "Add/Load ParamField to get more detail about this array element"));
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
        this.extraNamespaces.push("System");
        if (!this.type.arguments || this.type.arguments.length !== 1) {
            logWarning("unexpected arguments for array type, back to 'any'. arguments: " + JSON.stringify(this.type.arguments));
            this.arrayType = TypeDesc.AnyType;
        }
        else {
            this.arrayType = this.type.arguments[0];
        }
    }
}
