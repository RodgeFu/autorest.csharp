import { AzureResourceType } from "./AzureResourceType";

export class AzureResource implements AutoRest.SdkExplorer.Interface.AzureResource {
    name: string;
    id: string;
    region: string;
    type: AzureResourceType;
    resourceGroupName: string;
    subscriptionId: string;

    constructor(data: AutoRest.SdkExplorer.Interface.AzureResource) {
        this.name = data.name!;
        this.id = data.id!;
        this.region = data.region!;
        this.type = new AzureResourceType(data.type!);
        this.resourceGroupName = data.resourceGroupName!;
        this.subscriptionId = data.subscriptionId!;
    }
}

