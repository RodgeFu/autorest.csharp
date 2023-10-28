import { logError } from "../../Utils/logger";
import { MessageItem } from "../../Utils/messageItem";
import { TypeDesc } from "../Code/TypeDesc";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { SchemaEnumValue } from "../Schema/SchemaEnumValue";
import { ParamFieldBase, ParamFieldExtraConstructorParameters } from "./ParamFieldBase";
import { ParamFieldType } from "./ParamFieldFactory";


export class ParamFieldEnum extends ParamFieldBase {

    public set valueAsString(value: string | undefined) {
        // some example has space in region like West US;
        let v = this.normalizeEnumValue(value);
        let found = this.enumValues.find(ev => this.normalizeEnumValue(ev.value) == v || this.normalizeEnumValue(ev.internalValue) == v);       
        if (found === undefined) {
            this.lastMessage = new MessageItem(`Ignore invalid enum value '${v}' for enum type: ${this.type.fullNameWithNamespace}.`, "error");
        }
        else {
            this.value = found;
        }
    }
    public get valueAsString(): string | undefined {
        return this.selectedEnumValue;
    }

    public get selectedEnumValue(): string {
        if (this.value === undefined)
            return "null";
        else
            return this.value.value;
    }

    public get enumValues(): SchemaEnumValue[] {
        return this.type.schemaEnum?.values ?? [];
    }

    private normalizeEnumValue(v: string | undefined): string | undefined {
        if (!v)
            return undefined;
        return v.replaceAll(/[ _-]/g, "").toLowerCase();
    }

    protected getValueForCodeInternal(indent: string, formatter: CodeFormatter): string {
        if (this.selectedEnumValue === "default")
            return "default";
        return `${this.type.getFullNameWithNamespace(true /*includ global::*/, false /*include ending ?*/)}.${this.selectedEnumValue}`;
    }

    public override getDefaultValueWhenNotNull(): any {
        if (!this.enumValues || this.enumValues.length === 0) {
            logError("No enum value available for " + this.fieldName);
            return new SchemaEnumValue({
                value: "default",
                description: "",
                internalValue: "default"
            })
        }
        return this.enumValues[0];
    }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
    }
}
