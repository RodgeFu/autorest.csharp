declare module AutoRest.SdkExplorer.Interface {
	interface AzureResource {
		name?: string;
		id?: string;
		region?: string;
		type?: AutoRest.SdkExplorer.Interface.AzureResourceType;
		resourceGroupName?: string;
		subscriptionId?: string;
	}
}
