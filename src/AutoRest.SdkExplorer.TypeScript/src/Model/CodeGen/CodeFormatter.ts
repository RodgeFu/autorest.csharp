import { CodeSegmentDesc } from "../Code/CodeSegmentDesc";
import { ParameterDesc } from "../Code/ParameterDesc";
import { ParamFieldBase } from "../ParamField/ParamFieldBase";

export class CodeFormatter {
    private static _noFormmatter = new CodeFormatter();
    public static get noFormatter(): CodeFormatter {
        return CodeFormatter._noFormmatter;
    }

    constructor(
        public formatParamFieldCode?: (code: string, field: ParamFieldBase) => string) { }
}
