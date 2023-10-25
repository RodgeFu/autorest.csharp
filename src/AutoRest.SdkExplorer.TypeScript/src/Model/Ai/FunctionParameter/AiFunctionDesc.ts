import { AiObjectParamDesc } from "./AiObjectParamDesc";

export class AiFunctionDesc {

    constructor(
        public name: string,
        public description: string = "",
        public parameters: AiObjectParamDesc
    ) {
    }

    public toJson(minify: boolean = false): string {
        return JSON.stringify(this, (key: string, value: any) => {
            if (key === "isRequired" && value === false)
                return undefined;
            if (key === "required" && (value !== undefined && value.length === 0))
                return undefined;
            if (key === "type" && value === "enum")
                return undefined;
            return value;
        }, minify ? undefined : "  ");
    }
}
