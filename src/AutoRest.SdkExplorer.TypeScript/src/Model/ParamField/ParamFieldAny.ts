import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldType } from "./ParamFieldFactory";

export class ParamFieldAny extends ParamFieldBase {

    protected getValueForCodeInternal(indent: string, formatter: CodeFormatter): string {
        return this.valueAsString ?? "default";
    }

    public set valueAsString(str: string | undefined) {
        this.value = str;
    }
    public get valueAsString(): string | undefined {
        return this.value;
    }

    public override getDefaultValueWhenNotNull(): any {
        return "default";
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
    }
}
