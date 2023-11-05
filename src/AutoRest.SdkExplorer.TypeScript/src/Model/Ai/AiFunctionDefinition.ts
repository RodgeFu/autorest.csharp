import { isStringNullOrEmpty, tryJsonToObj } from "../../Utils/utils";
import { AiObjectParamDesc } from "./FunctionParameter/AiObjectParamDesc";

export class AiFunctionDefinition implements AutoRest.SdkExplorer.Interface.AiFunctionDefinition {
    name?: string;
    description?: string;
    parametersAsString?: string;

    constructor(data: AutoRest.SdkExplorer.Interface.AiFunctionDefinition) {
        this.name = data.name;
        this.description = data.description
        this.parametersAsString = data.parametersAsString;
    }

    //public static fromAiFunctionDesc(func: AiFunctionDesc) {
    //    return new AiFunctionDefinition({
    //        name: func.name,
    //        description: func.description,
    //        parametersAsString: func.parametersToJson()
    //    })
    //}

    public static create(name: string, description: string | undefined, parameters: AiObjectParamDesc) {
        return new AiFunctionDefinition({
            name: name,
            description: description,
            parametersAsString: JSON.stringify(parameters, AiFunctionDefinition.replacerFunc)
        })
    }

    public toJsonForOpenAi(minify: boolean = false) {
        const r = {
            name: this.name,
            description: this.description,
            parameters: isStringNullOrEmpty(this.parametersAsString) ? this.parametersAsString : tryJsonToObj(this.parametersAsString!)
        };
        return JSON.stringify(r, AiFunctionDefinition.replacerFunc, minify? undefined : "  ");
    }

    private static replacerFunc(key: string, value: any) {
        if (key === "isRequired" && value === false)
            return undefined;
        if (key === "required" && (value !== undefined && value.length === 0))
            return undefined;
        if (key === "type" && value === "enum")
            return undefined;
        return value;
    }
}
