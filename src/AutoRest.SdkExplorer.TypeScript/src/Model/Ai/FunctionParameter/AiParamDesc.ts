
export class AiParamDesc {

    constructor(
        public type: string,
        public description: string = "") {

        if(this.type)
            this.type = this.toShortTypeName(this.type);
        this.description = this.description.replace(".\"", "\"").replace(/[.;,] /g, ",").replace("<br>", "").replace("  ", "");
    }

    private toShortTypeName(type: string): string {
    switch (type) {
        case "System.String":
            return "string";
        case "System.Boolean":
            return "boolean";
        case "System.Decimal":
        case "System.Double":
        case "System.Single":
            return "number";
        case "System.Int32":
        case "System.UInt32":
        case "System.Int64":
        case "System.UInt64":
        case "System.Int16":
        case "System.UInt16":
            return "integer";
        default: return type;
    }
}

}
