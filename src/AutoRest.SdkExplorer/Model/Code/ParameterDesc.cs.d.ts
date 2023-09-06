declare module AutoRest.SdkExplorer.Interface {
	interface ParameterDesc extends PlaceHolderDesc {
		modelName: string;
		serializerName: string;
		type: AutoRest.SdkExplorer.Interface.TypeDesc;
		description: string;
		requestPath: string;
		defaultValue: string;
		source: string;
		sourceArg: string;
		isInPropertyBag: boolean;
	}
}
