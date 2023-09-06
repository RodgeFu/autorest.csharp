declare module AutoRest.SdkExplorer.Interface {
	interface SchemaProperty {
		accessibility: string;
		type: AutoRest.SdkExplorer.Interface.TypeDesc;
		name: string;
		description: string;
		serializerPath: string;
		isRequired: boolean;
		isReadonly: boolean;
		isWritableThroughCtor: boolean;
		isFlattenedProperty: boolean;
	}
}
