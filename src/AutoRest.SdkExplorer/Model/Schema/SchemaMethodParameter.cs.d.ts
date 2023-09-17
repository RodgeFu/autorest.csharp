declare module AutoRest.SdkExplorer.Interface {
	interface SchemaMethodParameter {
		name?: string;
		relatedPropertySerializerPath?: string;
		type?: AutoRest.SdkExplorer.Interface.TypeDesc;
		isOptional?: boolean;
		defaultValue?: string;
		description?: string;
	}
}
