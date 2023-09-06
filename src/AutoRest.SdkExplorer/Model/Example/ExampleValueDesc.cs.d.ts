declare module AutoRest.SdkExplorer.Interface {
	interface ExampleValueDesc {
		serializerName: string;
		modelName: string;
		/** Refer to AllSchemaTypes in autorest.csharp */
		schemaType: string;
		cSharpName: string;
		rawValue: string;
		propertyValues: { [index: string]: any };
		arrayValues: AutoRest.SdkExplorer.Interface.ExampleValueDesc[];
		hasRawValue: boolean;
		hasPropertyValues: boolean;
		hasArrayValues: boolean;
	}
}
