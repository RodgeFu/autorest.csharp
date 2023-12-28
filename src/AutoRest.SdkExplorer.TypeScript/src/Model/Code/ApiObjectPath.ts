
export class ApiObjectPath implements AutoRest.SdkExplorer.Interface.ApiObjectPath {
    static SEP: string = '/';

    public serviceName: string;

    public resourceName: string | undefined;

    public operationFamilyName: string | undefined;

    public version: string | undefined;

    public get path(): string {
        let r = this.serviceName;
        if (this.resourceName)
            r += `${ApiObjectPath.SEP}${this.resourceName}`;
        if (this.operationFamilyName)
            r += `${ApiObjectPath.SEP}${this.operationFamilyName}`;
        if (this.version)
            r += `${ApiObjectPath.SEP}${this.version}`;
        return r;
    }

    public get pathEscapedForHtmlId(): string {
        let NORMALIZED_SEP: string = "-";
        let normalize = (v: string) => v.replace(/\s+/g, "_").toLowerCase();

        let r = normalize(this.serviceName);
        if (this.resourceName)
            r += `${NORMALIZED_SEP}${normalize(this.resourceName)}`;
        if (this.operationFamilyName)
            r += `${NORMALIZED_SEP}${normalize(this.operationFamilyName)}`;
        if (this.version)
            r += `${NORMALIZED_SEP}${normalize(this.version)}`;
        return r;
    }

    constructor(data: AutoRest.SdkExplorer.Interface.ApiObjectPath) {
        this.serviceName = data.serviceName ?? "";
        this.resourceName = data.resourceName;
        this.operationFamilyName = data.operationFamilyName;
        this.version = data.version;
    }

    public static create(serviceName: string, resourceName?: string, operationFamilyName?: string, version?: string) {
        return new ApiObjectPath({
            serviceName: serviceName,
            resourceName: resourceName,
            operationFamilyName: operationFamilyName,
            version: version
        });
    }

    public static createFromPathString(path: string): ApiObjectPath | undefined {
        if (!path || path.length === 0)
            return undefined;
        let arr: string[] = path.split(ApiObjectPath.SEP, 4);

        let service = arr[0];
        let resource = arr.length > 1 ? arr[1] : undefined;
        let operation = arr.length > 2 ? arr[2] : undefined;
        let version = arr.length > 3 ? arr[3] : undefined;
        return new ApiObjectPath({
            serviceName: service,
            resourceName: resource,
            operationFamilyName: operation,
            version: version
        });
    }

    public static createFromParent(parent: ApiObjectPath | undefined, curName: string) {
        if (!parent || !parent.serviceName)
            return ApiObjectPath.create(curName);
        if (!parent.resourceName)
            return ApiObjectPath.create(parent.serviceName, curName);
        if (!parent.operationFamilyName)
            return ApiObjectPath.create(parent.serviceName, parent.resourceName, curName);
        if (!parent.version)
            return ApiObjectPath.create(parent.serviceName, parent.resourceName, parent.operationFamilyName, curName);
        throw 'Unexpected Path Parent';
    }

}
