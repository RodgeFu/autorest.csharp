import { SchemaBase } from "./SchemaBase";
import { SchemaEnumValue } from "./SchemaEnumValue";

export class SchemaEnum extends SchemaBase implements AutoRest.SdkExplorer.Interface.SchemaEnum {
    values: SchemaEnumValue[];
    description: string;


    constructor(data: AutoRest.SdkExplorer.Interface.SchemaEnum) {
        super(data)
        this.values = data.values!.map(d => new SchemaEnumValue(d));
        this.description = data.description ?? "";
    }

}
