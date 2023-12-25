declare module AutoRest.SdkExplorer.Interface {
	interface ExampleDesc {
		language?: string;
		sdkPackageName?: string;
		sdkPackageVersion?: string;
		explorerCodeGenVersion?: string;
		generatedTimestamp?: string;
		serviceName?: string;
		resourceName?: string;
		operationName?: string;
		swaggerOperationId?: string;
		sdkOperationId?: string;
		sdkFullUniqueName?: string;
		exampleName?: string;
		originalFilePath?: string;
		originalFileNameWithoutExtension?: string;
		aiDescription?: string;
		embeddingText?: string;
		embeddingVector?: string;
		exampleValues?: { [index: string]: AutoRest.SdkExplorer.Interface.ExampleValueDesc };
	}
}
