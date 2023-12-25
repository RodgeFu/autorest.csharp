declare module AutoRest.SdkExplorer.Interface {
	interface ProviderType {
	}
	interface ApiDesc {
		language?: string;
		sdkPackageName?: string;
		sdkPackageVersion?: string;
		explorerCodeGenVersion?: string;
		generatedTimestamp?: string;
		dependencies?: string[];
		serviceName?: string;
		resourceName?: string;
		operationName?: string;
		swaggerOperationId?: string;
		sdkOperationId?: string;
		description?: string;
		apiDescription?: string;
		aiDescription?: string;
		fullUniqueName?: string;
		encodedFunctionName?: string;
		operationNameWithParameters?: string;
		operationNameWithScopeAndParameters?: string;
		embeddingText?: string;
		embeddingVector?: string;
		/** Include parameter in or not in propertybag; */
		operationMethodParameters?: AutoRest.SdkExplorer.Interface.ParameterDesc[];
		codeSegments?: AutoRest.SdkExplorer.Interface.CodeSegmentDesc[];
		operationProviderAzureResourceType?: AutoRest.SdkExplorer.Interface.AzureResourceType;
		operationProviderType?: string;
		requestPath?: string;
		apiVersion?: string;
		schemaStore?: AutoRest.SdkExplorer.Interface.SchemaStore;
	}
}
