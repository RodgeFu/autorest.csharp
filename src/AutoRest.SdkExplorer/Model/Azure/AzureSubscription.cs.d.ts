declare module AutoRest.SdkExplorer.Interface {
	interface AzureSubscription {
		resourceGroups?: AutoRest.SdkExplorer.Interface.AzureResourceGroup[];
		resources?: AutoRest.SdkExplorer.Interface.AzureResource[];
		subscriptionId?: string;
		name?: string;
		id?: string;
		isDefault?: boolean;
	}
}
