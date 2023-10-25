import { AiParamDesc } from "./AiParamDesc";

export class AiEnumParamDesc extends AiParamDesc {

    constructor(description: string, enumValues: string[]) {
        super("enum", description);

        this.enumValues = enumValues;
        //const cleanup = (str: string) => {
        //    return str.replace(/[\ \-_]/g, "").toLowerCase();
        //}

        //this.possibleEnumValues.forEach(ev => {
        //    if (cleanup(ev.description) == cleanup(ev.value)) {
        //        ev.description = "";
        //    }
        //})
    }

    get enumValues(): string[] {
        return (this as any)["enum"];
    }
    set enumValues(value: string[]) {
        (this as any)["enum"] = value;
    }

}
