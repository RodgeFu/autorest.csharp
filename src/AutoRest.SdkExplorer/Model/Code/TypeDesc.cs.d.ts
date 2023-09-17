declare module AutoRest.SdkExplorer.Interface {
	interface TypeDesc {
		name?: string;
		namespace?: string;
		isValueType?: boolean;
		isEnum?: boolean;
		isNullable?: boolean;
		isGenericType?: boolean;
		isFrameworkType?: boolean;
		isList?: boolean;
		isDictionary?: boolean;
		isBinaryData?: boolean;
		arguments?: AutoRest.SdkExplorer.Interface.TypeDesc[];
		fullNameWithNamespace?: string;
		fullNameWithoutNamespace?: string;
		schemaKey?: string;
		schemaType?: string;
	}
}
