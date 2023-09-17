import moment from "moment";
import { MessageItem } from "../../Utils/messageItem";
import { ParamFieldBase, ParamFieldExtraConstructorParameters, ValidationResult } from "./ParamFieldBase";
import { DATE_FORMAT } from "../../Utils/utils";
import { TypeDesc } from "../Code/TypeDesc";
import { ParamFieldType } from "./ParamFieldFactory";
import { CodeFormatter } from "../CodeGen/CodeFormatter";

export class ParamFieldDate extends ParamFieldBase {
  public override validateValue(): ValidationResult {
    if (this.value == "")
      return {
        pass: false,
        message: new MessageItem("Invalid Date", "error"),
        valueToValidate: this.value
      }
    else
      return {
        pass: true,
        message: new MessageItem(),
        valueToValidate: this.value
      };
  }
  public set valueAsString(str: string | undefined) {
    if (str !== undefined && str.length === 0)
      this.value = moment();
    else
      this.value = (str === undefined) ? undefined : moment(str);
  }
  public get valueAsString(): string | undefined {
    let d = this.value as moment.Moment;
    if (d)
      return d.format(DATE_FORMAT)
    else {
      return undefined;
    }
  }

    protected getValueForCodeInternal(indent: string, formatter: CodeFormatter): string {
    return this.valueAsString === undefined ? "null" : `global::System.DateTimeOffset.Parse("${this.valueAsString}")`;
  }

  public override getDefaultValueWhenNotNull(): any {
    return moment();
  }

    constructor(fieldName: string, fieldType: ParamFieldType, type: TypeDesc, parent: ParamFieldBase | undefined, params: ParamFieldExtraConstructorParameters) {
        super(fieldName, fieldType, type, parent, params);
  }
}
