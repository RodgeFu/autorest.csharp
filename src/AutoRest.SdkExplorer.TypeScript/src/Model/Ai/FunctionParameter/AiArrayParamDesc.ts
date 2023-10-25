import { AiParamDesc } from "./AiParamDesc";

export class AiArrayParamDesc extends AiParamDesc {

    public items: AiParamDesc;

    constructor(
        description: string,
        items: AiParamDesc) {
        super("array", description);
        this.items = items;
    }
}
