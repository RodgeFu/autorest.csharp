import { AiArrayParamDesc } from "./AiArrayParamDesc";
import { AiObjectParamDesc } from "./AiObjectParamDesc";
import { AiParamDesc } from "./AiParamDesc";

export class AiDictParamDesc extends AiArrayParamDesc {

    constructor(
        description: string = "",
        keyType: AiParamDesc,
        valueType: AiParamDesc) {
        super(
            description,
            new AiObjectParamDesc("dictionary items as array", {
                "key": keyType,
                "value": valueType
            }));
    }
}
