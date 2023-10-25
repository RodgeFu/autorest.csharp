import { AiEnumParamDesc } from "./AiEnumParamDesc";

export class AiDefaultParamDesc extends AiEnumParamDesc {

    constructor(
        description: string,
        defaultValue: string) {
        super(description, [defaultValue])
    }
}
