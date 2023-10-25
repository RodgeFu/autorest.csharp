import { logWarning } from "../../../Utils/logger";
import { AiParamDesc } from "./AiParamDesc";

export class AiObjectParamDesc extends AiParamDesc {

    public properties: { [index: string]: AiParamDesc };
    public required: string[] | undefined;

    constructor(
        description: string,
        properties: { [index: string]: AiParamDesc },
        required?: string[]
    ) {
        super("object", description);

        this.properties = properties;
        this.required = required;

        required?.forEach(f => {
            if (properties[f] === undefined) {
                logWarning("Ai required property not found in properties list: " + f);
            }
        });
    }

}
