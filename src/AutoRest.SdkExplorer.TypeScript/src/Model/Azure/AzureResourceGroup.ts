export class AzureResourceGroup implements AutoRest.SdkExplorer.Interface.AzureResourceGroup {
    name: string;
    id: string;
    region: string;

    constructor(data: AutoRest.SdkExplorer.Interface.AzureResourceGroup) {
        this.name = data.name!;
        this.id = data.id!;
        this.region = data.region!;
    }
}

