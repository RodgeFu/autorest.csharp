import { TypeDesc } from "../Code/TypeDesc";

export class SchemaProperty implements AutoRest.SdkExplorer.Interface.SchemaProperty
{
    accessibility: string;
    type: TypeDesc;
    name: string;
    description: string;
    serializerPath: string;
    isRequired: boolean;
    isReadonly: boolean;
    isWritableThroughCtor: boolean;
    isFlattenedProperty: boolean;

    constructor(data: AutoRest.SdkExplorer.Interface.SchemaProperty) {
        this.accessibility = data.accessibility!;
        this.type = new TypeDesc(data.type!);
        this.name = data.name!;
        this.description = data.description!;
        this.serializerPath = data.serializerPath!;
        this.isRequired = data.isRequired!;
        this.isReadonly = data.isReadonly!;
        this.isWritableThroughCtor = data.isWritableThroughCtor!;
        this.isFlattenedProperty = data.isFlattenedProperty!;
    }
}
