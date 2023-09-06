declare module AutoRest.SdkExplorer.Interface {
	interface FieldHint {
		key: string;
		azureResourceTypes: AutoRest.SdkExplorer.Interface.AzureResourceType[];
		rawExampleValues: AutoRest.SdkExplorer.Interface.ExampleRawValueHint[];
	}
}
