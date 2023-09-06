declare module AutoRest.SdkExplorer.Interface {
	interface ProviderType {
	}
	interface ApiDesc {
		language: string;
		sdkPackageName: string;
		sdkPackageVersion: string;
		explorerCodeGenVersion: string;
		generatedTimestamp: string;
		dependencies: string[];
		serviceName: string;
		resourceName: string;
		operationName: string;
		swaggerOperationId: string;
		sdkOperationId: string;
		description: string;
		fullUniqueName: string;
		operationNameWithParameters: string;
		operationNameWithScopeAndParameters: string;
		/** Include parameter in or not in propertybag; */
		operationMethodParameters: AutoRest.SdkExplorer.Interface.ParameterDesc[];
		codeSegments: AutoRest.SdkExplorer.Interface.CodeSegmentDesc[];
		operationProviderAzureResourceType: AutoRest.SdkExplorer.Interface.AzureResourceType;
		operationProviderType: string;
		requestPath: string;
		apiVersion: string;
		schemaObjects: AutoRest.SdkExplorer.Interface.SchemaObject[];
		schemaEnums: AutoRest.SdkExplorer.Interface.SchemaEnum[];
		schemaNones: AutoRest.SdkExplorer.Interface.SchemaNone[];
	}
}
