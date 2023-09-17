export class AzureResourceType implements AutoRest.SdkExplorer.Interface.AzureResourceType {
    namespace: string;
    type: string;

    constructor(data: AutoRest.SdkExplorer.Interface.AzureResourceType) {
        this.namespace = data.namespace ?? "UnknownNamespace";
        this.type = data.type ?? "UnknownType";
    }

    public toString(toLower: boolean = false): string {
        return toLower ? `${this.namespace.toLowerCase()}/${this.type.toLowerCase()}` : `${this.namespace}/${this.type}`;
    }

    public get hasParentResourceType(): boolean {
        return this.type.indexOf("/") >= 0;
    }

    public get fullTypeName(): string {
        return this.toString(false);
    }

    public isEquals(res: AzureResourceType): boolean {
        if (!res)
            return false;
        return this.toString(true) === res.toString(true);
    }

    public static isEquals(a: AzureResourceType | undefined, b: AzureResourceType | undefined): boolean {
        if (a === undefined && b === undefined)
            return true;
        if (a === undefined || b === undefined)
            return false;
        return a.isEquals(b);
    }
}
