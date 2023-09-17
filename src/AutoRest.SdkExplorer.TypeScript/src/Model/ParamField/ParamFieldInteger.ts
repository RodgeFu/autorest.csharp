import { logError } from "../../Utils/logger";
import { MessageItem } from "../../Utils/messageItem";
import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { ParamFieldBase, ParamFieldExtraConstructorParameters, ValidationResult } from "./ParamFieldBase";
import { ParamFieldType } from "./ParamFieldFactory";

export class ParamFieldInteger extends ParamFieldBase {
    public set valueAsString(str: string | undefined) {
        if (str === undefined)
            this.value = undefined;
        else if (!str || str === "0") {
            this.value = 0;
        }
        else {
            let r = parseInt(str);
            if (isNaN(r)) {
                logError("Try to set Int to NaN: " + str);
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

    public override validateValue(): ValidationResult {
        let vu = parseInt(this.value);
        if (Number.isNaN(vu)) {
            return {
                pass: false,
                valueToValidate: this.value,
                message: new MessageItem("Invalid Input for Integer", "error")
            };
        }
        else {
            return {
                pass: true,
                valueToValidate: this.value,
                message: new MessageItem()
            };
        }
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
    }
}
