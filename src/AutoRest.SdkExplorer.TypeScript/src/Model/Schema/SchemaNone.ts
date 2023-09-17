import { SchemaBase } from "./SchemaBase";

export class SchemaNone extends SchemaBase implements AutoRest.SdkExplorer.Interface.SchemaNone {
    reason: string;

    constructor(data: AutoRest.SdkExplorer.Interface.SchemaNone) {
        super(data);
        this.reason = data.reason!;
    }
}
