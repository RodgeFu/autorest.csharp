declare module AutoRest.SdkExplorer.Interface {
	interface SchemaObject extends SchemaBase {
		properties: AutoRest.SdkExplorer.Interface.SchemaProperty[];
		inheritFrom: AutoRest.SdkExplorer.Interface.TypeDesc;
		/** TODO: discriminator support */
		inheritBy: AutoRest.SdkExplorer.Interface.TypeDesc[];
		initializationConstructor: AutoRest.SdkExplorer.Interface.SchemaMethod;
		serializationConstructor: AutoRest.SdkExplorer.Interface.SchemaMethod;
		staticCreateMethod: AutoRest.SdkExplorer.Interface.SchemaMethod;
		description: string;
		isStruct: boolean;
		isEnum: boolean;
		enumValues: AutoRest.SdkExplorer.Interface.SchemaEnumValue[];
		discriminatorKey: string;
		isDiscriminatorBase: boolean;
		discriminatorProperty: AutoRest.SdkExplorer.Interface.SchemaProperty;
		defaultConstructor: AutoRest.SdkExplorer.Interface.SchemaMethod;
	}
}
