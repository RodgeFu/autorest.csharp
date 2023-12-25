import { SchemaBase } from "./SchemaBase";

export class SchemaEnumValue implements AutoRest.SdkExplorer.Interface.SchemaEnumValue {
    value: string;
    internalValue?: string;
    description: string;

    constructor(data: AutoRest.SdkExplorer.Interface.SchemaEnumValue) {
        this.value = data.value?.toString()!;
        this.internalValue = data.internalValue?.toString();
        this.description = data.description ?? "";
    }
}
