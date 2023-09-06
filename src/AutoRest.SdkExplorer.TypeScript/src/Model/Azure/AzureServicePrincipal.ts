export class AzureServicePrincipal implements AutoRest.SdkExplorer.Interface.AzureServicePrincipal {
    name: string;
    appId: string;
    tenantId: string;

    constructor(data: AutoRest.SdkExplorer.Interface.AzureServicePrincipal) {
        this.name = data.name;
        this.appId = data.appId;
        this.tenantId = data.tenantId;
    }
}

