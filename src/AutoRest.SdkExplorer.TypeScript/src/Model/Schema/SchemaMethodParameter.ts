import { TypeDesc } from "../Code/TypeDesc";

export class SchemaMethodParameter implements AutoRest.SdkExplorer.Interface.SchemaMethodParameter {
    name: string;
    relatedPropertySerializerPath: string;
    type: TypeDesc;
    isOptional: boolean;
    defaultValue?: string;
    description: string;

    constructor(data: AutoRest.SdkExplorer.Interface.SchemaMethodParameter) {
        this.name = data.name!;
        this.relatedPropertySerializerPath = data.relatedPropertySerializerPath!;
        this.type = new TypeDesc(data.type!);
        this.isOptional = data.isOptional!;
        this.defaultValue = data.defaultValue;
        this.description = data.description ?? "";
    }
}
