import { AzureResource } from "./AzureResource";
import { AzureResourceGroup } from "./AzureResourceGroup";

export class AzureSubscription implements AutoRest.SdkExplorer.Interface.AzureSubscription {
    resourceGroups: AzureResourceGroup[];
    resources: AzureResource[];
    subscriptionId: string;
    name: string;
    id: string;
    isDefault: boolean;

    constructor(data: AutoRest.SdkExplorer.Interface.AzureSubscription) {
        this.resourceGroups = data.resourceGroups?.map(rg => new AzureResourceGroup(rg)) ?? [];
        this.resources = data.resources?.map(r => new AzureResource(r)) ?? [];
        this.subscriptionId = data.subscriptionId!;
        this.name = data.name!;
        this.id = data.id!;
        this.isDefault = data.isDefault ?? false;
    }
}
