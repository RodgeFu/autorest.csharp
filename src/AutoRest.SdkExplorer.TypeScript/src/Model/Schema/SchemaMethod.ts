import { SchemaMethodParameter } from "./SchemaMethodParameter";

export class SchemaMethod implements AutoRest.SdkExplorer.Interface.SchemaMethod {
    name: string;
    methodParameters: SchemaMethodParameter[];

    constructor(data: AutoRest.SdkExplorer.Interface.SchemaMethod) {
        this.name = data.name!;
        this.methodParameters = data.methodParameters?.map(d => new SchemaMethodParameter(d)) ?? [];
    }
}
