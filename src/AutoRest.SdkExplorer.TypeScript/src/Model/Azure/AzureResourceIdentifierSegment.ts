import { isStringEqualCaseInsensitive } from "../../Utils/utils";

export class AzureResourceIdentifierSegment implements AutoRest.SdkExplorer.Interface.AzureResourceIdentifierSegment {
    type: string;
    value: string;

    get isParameterized(): boolean { return this.value.startsWith("{") && this.value.endsWith("}"); }

    constructor(data: AutoRest.SdkExplorer.Interface.AzureResourceIdentifierSegment) {
        this.type = data.type;
        this.value = data.value;
    }

    public isParameter(parameterName: string): boolean {
        if (!this.isParameterized)
            return false;
        const n = this.value.slice(1, this.value.length - 1);
        return isStringEqualCaseInsensitive(n, parameterName);
    }
}

