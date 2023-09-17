import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldType } from "./ParamFieldFactory";

export class ParamFieldBinary extends ParamFieldBase {

    public set valueAsString(str: string | undefined) {
        this.value = str;
    }
    public get valueAsString(): string | undefined {
        return this.value;
    }

    protected getValueForCodeInternal(indent: string, formatter: CodeFormatter): string {
        // TODO: only support base64 for now, add more support when needed
        return this.valueAsString === undefined ? "null" : `global::System.Convert.ToBase64String(Encoding.UTF8.GetBytes("${this.valueAsString}"))`;
    }

    public override getDefaultValueWhenNotNull(): any {
        return "";
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
        this.extraNamespaces.push("System");
    }
}
