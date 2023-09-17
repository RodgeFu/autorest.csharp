import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldType } from "./ParamFieldFactory";

export class ParamFieldString extends ParamFieldBase {

    public set valueAsString(str: string | undefined) {
        this.value = str;
    }
    public get valueAsString(): string | undefined {
        return this.value;
    }

    protected getValueForCodeInternal(indent: string, formatter: CodeFormatter): string {
        let str = this.valueAsString;
        if (str === undefined)
            return "";
        let refName = ParamFieldBase.fromRef(str);
        if (refName)
            return refName;
        return `@"${str.replaceAll('"', '\\"')}"`;
    }

    public override getDefaultValueWhenNotNull(): any {
        return "";
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
    }
}
