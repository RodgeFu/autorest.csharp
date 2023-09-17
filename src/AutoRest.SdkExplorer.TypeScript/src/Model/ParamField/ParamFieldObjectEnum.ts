import { TypeDesc } from "../Code/TypeDesc";
import { SchemaEnumValue } from "../Schema/SchemaEnumValue";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldEnum } from "./ParamFieldEnum";
import { ParamFieldType } from "./ParamFieldFactory";

export class ParamFieldObjectEnum extends ParamFieldEnum {

    public override get enumValues(): SchemaEnumValue[] {
        return this.type.schemaObject?.enumValues ?? [];
    }

    public override getDefaultValueWhenNotNull(): any {
        return this.enumValues[0];
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
    }
}
