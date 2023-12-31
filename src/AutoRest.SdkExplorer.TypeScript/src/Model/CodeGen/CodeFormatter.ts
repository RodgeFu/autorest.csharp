import { CodeSegmentDesc } from "../Code/CodeSegmentDesc";
import { ParameterDesc } from "../Code/ParameterDesc";
import { ParamFieldBase } from "../ParamField/ParamFieldBase";

export class NoCodeFormatter implements CodeFormatter {

    constructor() { }

    public formatParamFieldCode(code: string, field: ParamFieldBase): string {
        return code;
    }
}

export abstract class CodeFormatter {
    private static _noFormmatter = new NoCodeFormatter();
    public static get noFormatter(): CodeFormatter {
        return CodeFormatter._noFormmatter;
    }

    constructor() { }

    public abstract formatParamFieldCode(code: string, field: ParamFieldBase): string;
}
