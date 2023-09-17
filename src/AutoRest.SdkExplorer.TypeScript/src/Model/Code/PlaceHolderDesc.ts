
export class PlaceHolderDesc implements AutoRest.SdkExplorer.Interface.PlaceHolderDesc {
    key: string;
    suggestedName: string;

    private _nameToUse?: string;
    get nameToUse(): string {
        return this._nameToUse ?? this.suggestedName;
    }
    set nameToUse(value: string) {
        this._nameToUse = value;
    }

    constructor(data: AutoRest.SdkExplorer.Interface.PlaceHolderDesc) {
        this.key = data.key!;
        this.suggestedName = data.suggestedName!;
    }
}
