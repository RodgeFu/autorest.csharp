import { TypeDesc } from "../Code/TypeDesc";
import { ExampleValueDesc } from "../Example/ExampleValueDesc";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldType } from "./ParamFieldFactory";
import { ParamFieldObject } from "./ParamFieldObject";

export class ParamFieldObjectOneProperty extends ParamFieldObject {

    public override setExampleValue(value: ExampleValueDesc): void {

        // some one property object will be given example value directly, like ResourceIdentifier
        // TODO: double check whether generate code is opitimized too
        if ((value.propertyValues === undefined || value.propertyValuesMap === undefined || value.propertyValuesMap.size === 0) && value.rawValue !== undefined) {
            let list = this.getDefaultValueWhenNotNull() as ParamFieldBase[];
            list[0].setExampleValue(value);
            this.valueAsProperties = list;
            //this.refreshRelatedExamples();
        }
        else {
            super.setExampleValue(value);
        }
    }

    public override linkRelatedExamples(examples: ExampleValueDesc[]): void {
        this.relatedExamples = examples;

        if (this.oneProperty) {
            let arr: ExampleValueDesc[] = [];
            examples.forEach(ex => {
                if ((ex.propertyValues === undefined || ex.propertyValuesMap === undefined || ex.propertyValuesMap.size === 0) && ex.rawValue !== undefined) {
                    arr.push(ex);
                }
            });
            if (arr.length > 0) {
                this.oneProperty.linkRelatedExamples(arr);
            }
            else {
                super.linkRelatedExamples(examples);
            }
        }
    }

    public get oneProperty(): ParamFieldBase | undefined {
        if (this.valueAsProperties === undefined || this.valueAsProperties.length === 0)
            return undefined;
        return this.valueAsProperties[0];
    }

    public override set valueAsString(str: string | undefined) {
        if (str === undefined) {
            this.value = undefined;
        }
        else {
            if (this.isNull)
                this.isNull = false;
            // gProperty won't be undefined when isNull is set to false;
            this.oneProperty!.valueAsString = str;
        }
    }
    public override get valueAsString(): string | undefined {
        return this.oneProperty?.valueAsString;
    }

    public override get id(): string {
        if (this.oneProperty)
            return this.oneProperty.id;
        else {
            let r = this.getDefaultValueWhenNotNull() as ParamFieldBase[];
            // use the only property's id as mine to link the code part and my ui fields
            if (r.length === 1)
                return r[0].id;
            else {
                console.error("Property length is not 1 in ParamFieldOneProperty: " + this.type.fullNameWithNamespace);
                return this.id;
            }
        }
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
    }
}
