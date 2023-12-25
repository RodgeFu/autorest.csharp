
export class AiParamDesc {

    public format: string | undefined;

    constructor(
        public type: string,
        public description: string = "") {

        this.initType();
        this.description = this.description.replace(".\"", "\"").replace(/[.;,] /g, ",").replace("<br>", "").replace("  ", "");
    }

    private initType() {
        switch (this.type) {
            case "System.String":
                this.type = "string";
                break;
            case "System.Boolean":
                this.type = "boolean";
                break;
            case "System.Decimal":
            case "System.Double":
            case "System.Single":
                this.type = "number";
                break;
            case "System.Int32":
            case "System.UInt32":
            case "System.Int64":
            case "System.UInt64":
            case "System.Int16":
            case "System.UInt16":
                this.type = "integer";
                break;
            case "System.DateTimeOffset":
                this.type = "string";
                this.format = "date-time";
                break;
            case "System.BinaryData":
            case "System.Byte[]":
                this.type = "string";
                break;
            default:

        }
    }

}
