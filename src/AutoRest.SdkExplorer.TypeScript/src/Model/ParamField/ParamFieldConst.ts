import { AiPayloadApplyOutput } from "../Ai/FunctionParameter/AiPayloadApplyOutput";
import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldType } from "./ParamFieldFactory";

export class ParamFieldConst extends ParamFieldBase {
    private _constValue?: string;

    protected getValueForCodeInternal(indent: string, formatter: CodeFormatter): string {
        return this._constValue ?? "default";
    }

    public set valueAsString(str: string | undefined) {
        
    }
    public get valueAsString(): string | undefined {
        return this._constValue;
    }

    public override getDefaultValueWhenNotNull(): any {
        return this._constValue ?? "default";
    }

    public override generateAiPayloadInternal(): any {
        return this._constValue;
    }

    protected override applyAiPayloadInternal(payload: any, output: AiPayloadApplyOutput) {
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, constValue: string | undefined, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
        this._constValue = constValue;
    }
}
