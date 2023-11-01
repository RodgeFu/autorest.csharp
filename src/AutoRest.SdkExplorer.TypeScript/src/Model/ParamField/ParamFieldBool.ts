import { AiPayloadApplyOutput } from "../Ai/FunctionParameter/AiPayloadApplyOutput";
import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldType } from "./ParamFieldFactory";

export class ParamFieldBool extends ParamFieldBase {
    public set valueAsString(v: string | undefined) {
        if (v === undefined)
            this.value = v;
        else
            this.value = v.toLowerCase() === "true" ? true : false;
    }

    public get valueAsString(): string | undefined {
        if (this.value === undefined)
            return undefined;
        else
            return this.value === true ? "true" : "false";
    }

    protected override applyAiPayloadInternal(payload: any, output: AiPayloadApplyOutput) {
        if (payload === true || payload === false)
            this.value = payload;
        else
            super.applyAiPayloadInternal(payload, output);
    }

    protected getValueForCodeInternal(indent: string, formatter: CodeFormatter): string {
        return this.value === true ? "true" : "false";
    }

    public override getDefaultValueWhenNotNull(): any {
        return false;
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
    }
}
