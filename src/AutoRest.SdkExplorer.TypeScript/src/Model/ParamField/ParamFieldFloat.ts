import { logError } from "../../Utils/logger";
import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldType } from "./ParamFieldFactory";

export class ParamFieldNumber extends ParamFieldBase {
    public set valueAsString(str: string | undefined) {
        if (str === undefined)
            this.value = undefined;
        else if (!str || str === "0") {
            this.value = 0;
        }
        else {
            let r = parseFloat(str);
            if (isNaN(r)) {
                logError("Try to set Float to NaN: " + str);
                this.value = undefined;
            }
            else
                this.value = r;
        }
    }
    public get valueAsString(): string | undefined {
        if (this.value === undefined)
            return undefined;
        else if (!this.value)
            return "0";
        else
            return this.value.toString();
    }

    protected getValueForCodeInternal(indent: string, formatter: CodeFormatter): string {
        return this.valueAsString!;
    }

    public override getDefaultValueWhenNotNull(): any {
        return 0;
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
    }
}
