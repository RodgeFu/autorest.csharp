import { ParamFieldBase } from "../ParamField/ParamFieldBase";
import { ParamFieldFactory } from "../ParamField/ParamFieldFactory";
import { SchemaStore } from "../Schema/SchemaStore";
import { PlaceHolderDesc } from "./PlaceHolderDesc";
import { TypeDesc } from "./TypeDesc";

export class ParameterDesc extends PlaceHolderDesc implements AutoRest.SdkExplorer.Interface.ParameterDesc {
    modelName: string;
    serializerName: string;
    type: TypeDesc;
    description: string;
    requestPath?: string;
    defaultValue?: string;
    source?: string;
    sourceArg?: string;
    isInPropertyBag: boolean;

    constructor(data: AutoRest.SdkExplorer.Interface.ParameterDesc) {
        super(data);
        this.modelName = data.modelName!;
        this.serializerName = data.serializerName!;
        this.type = new TypeDesc(data.type!);
        this.description = data.description ?? "";
        this.requestPath = data.requestPath;
        this.defaultValue = data.defaultValue;
        this.source = data.source;
        this.sourceArg = data.sourceArg;
        this.isInPropertyBag = data.isInPropertyBag ?? false;

    }

    public loadSchema(schemaStore: SchemaStore) {
        this.type?.loadSchema(schemaStore);
    }

    public createParamField(stepName: string): ParamFieldBase {

        let paramParamField = ParamFieldFactory.createParamField(this.suggestedName!, this.type!, undefined,
            { isReadonly: false, isRequired: true, serializerPath: this.suggestedName!, parameterOwner: this, idPrefix: stepName });
        if (paramParamField === undefined) {
            console.error("failed to create param field for " + this.suggestedName);
        }
        // no need to set when the default value is not available or just default
        if (this.defaultValue !== undefined && this.defaultValue !== '__N/A__' && this.defaultValue !== 'default') {
            paramParamField.valueAsString = this.defaultValue;
        }
        paramParamField.description = this.description ?? "";
        return paramParamField;
    }
}
