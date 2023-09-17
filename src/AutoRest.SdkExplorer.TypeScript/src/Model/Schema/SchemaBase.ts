export class SchemaBase implements AutoRest.SdkExplorer.Interface.SchemaBase {
    schemaKey: string;
    schemaType: string;

    constructor(data: AutoRest.SdkExplorer.Interface.SchemaBase) {
        this.schemaKey = data.schemaKey!;
        this.schemaType = data.schemaType!;
    }

}
