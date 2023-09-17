import { ONE_INDENT } from "../../Utils/utils";
import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { ExampleValueDesc } from "../Example/ExampleValueDesc";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldDictionaryItem } from "./ParamFieldDictionaryItem";
import { ParamFieldFactory, ParamFieldType } from "./ParamFieldFactory";

export class ParamFieldDictionary extends ParamFieldBase {

    public readonly keyType: TypeDesc;
    public readonly valueType: TypeDesc;

    public override setExampleValue(value: ExampleValueDesc): void {
        if (value.propertyValues === undefined)
            this.valueAsDictionaryItemArray = undefined;
        else if (value.propertyValuesMap.size === 0)
            this.valueAsDictionaryItemArray = [];
        else {
            let arr: ParamFieldDictionaryItem[] = [];
            let index: number = 0;
            value.propertyValuesMap?.forEach((v, k) => {
                let itemName = this.generateItemName(index++);
                let r: ParamFieldBase = ParamFieldFactory.createParamField(itemName, TypeDesc.getDictionaryItemType(this.type), this,
                    { isReadonly: false, isRequired: false, serializerPath: itemName, parameterOwner: this.parameterOwner, idPrefix: this.idPrefix });
                //v.tag = k;
                r.setExampleValue(v);
                arr.push(r as ParamFieldDictionaryItem);
            });
            this.valueAsDictionaryItemArray = arr;
            //this.refreshRelatedExamples();
        }
    }

    public override linkRelatedExamples(examples: ExampleValueDesc[]): void {
        super.linkRelatedExamples(examples);

        let arr: ExampleValueDesc[] = [];
        examples.forEach(ex => {
            ex.propertyValuesMap?.forEach((v, k) => {
                //v.tag = k;
                arr.push(v);
            })
        });

        this.valueAsDictionaryItemArray?.forEach(item => {
            item.linkRelatedExamples(arr);
        })
    }

    public override getChildren(): ParamFieldBase[] {
        return this.valueAsDictionaryItemArray ?? [];
    }

    public get valueAsDictionaryItemArray(): ParamFieldDictionaryItem[] | undefined {
        return this.value;
    }
    public set valueAsDictionaryItemArray(dict: ParamFieldDictionaryItem[] | undefined) {
        this.value = dict;
    }

    public set valueAsString(str: string | undefined) {
        if (str === undefined || str.length === 0 || str === "default")
            this.value = undefined;
        else
            throw Error("set object through string not supported");
    }

    public get valueAsString(): string | undefined {
        return this.type.fullNameWithNamespace;
    }

    protected getValueForCodeInternal(indent: string, formatter: CodeFormatter): string {
        if (this.valueAsDictionaryItemArray === undefined)
            return "null";
        else if (this.valueAsDictionaryItemArray.length == 0)
            return this.isReadonly ? "{}" : `new global::System.Collections.Generic.Dictionary<${this.keyType.getFullNameWithNamespace(true /*includ global::*/, false /*include ending ?*/)}, ${this.valueType.getFullNameWithNamespace(true /*includ global::*/, true /*include ending ?*/)}>()`;
        else {
            let s = "";
            this.valueAsDictionaryItemArray.forEach((item) => s += `${indent + ONE_INDENT}${item.getValueForCode(indent + ONE_INDENT, formatter)},\n`);

            return (this.isReadonly ? "\n" : `new global::System.Collections.Generic.Dictionary<${this.keyType.getFullNameWithNamespace(true /*includ global::*/, false /*include ending ?*/)}, ${this.valueType.getFullNameWithNamespace(true /*includ global::*/, true /*include ending ?*/)}>()\n`) +
                `${indent}{\n` +
                s +
                `${indent}}`;
        }
    }

    public override getDefaultValueWhenNotNull(): any {
        return [];
    }

    public appendItem(): ParamFieldBase {
        if (this.valueAsDictionaryItemArray === undefined)
            this.valueAsDictionaryItemArray = [];
        let name = this.generateItemName(this.valueAsDictionaryItemArray.length);
        let item = ParamFieldFactory.createParamField(
            name, TypeDesc.getDictionaryItemType(this.type), this,
            { isReadonly: false, isRequired: true, serializerPath: name, parameterOwner: this.parameterOwner, idPrefix: this.idPrefix });
        this.valueAsDictionaryItemArray.push(item as ParamFieldDictionaryItem);
        this.refreshRelatedExamples();
        return item;
    }

    public removeItem(fieldName: string) {
        if (!this.valueAsDictionaryItemArray || this.valueAsDictionaryItemArray.length === 0)
            return;

        var newArray: ParamFieldDictionaryItem[] = [];
        let newIndex = 0;
        this.valueAsDictionaryItemArray?.forEach((v, index) => {
            if (v.fieldName !== fieldName) {
                v.fieldName = this.generateItemName(newIndex++);
                newArray.push(v);
            }
        })
        this.valueAsDictionaryItemArray = newArray;
    }

    private generateItemName(index: number) {
        return `Item${index}`;
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
        this.extraNamespaces.push("System.Collections.Generic");
        if (!this.type.arguments || this.type.arguments.length !== 2) {
            throw Error("unexpected arguments for dict item. type: " + this.type.fullNameWithNamespace);
        }
        else {
            this.keyType = this.type.arguments[0];
            this.valueType = this.type.arguments[1];
        }
    }
}
