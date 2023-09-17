declare module AutoRest.SdkExplorer.Interface {
	interface CodeSegmentDesc extends PlaceHolderDesc {
		code?: string;
		usingNamespaces?: string[];
		dependencies?: AutoRest.SdkExplorer.Interface.VariableDesc[];
		outputResult?: AutoRest.SdkExplorer.Interface.VariableDesc[];
		variables?: AutoRest.SdkExplorer.Interface.VariableDesc[];
		parameters?: AutoRest.SdkExplorer.Interface.ParameterDesc[];
		function?: AutoRest.SdkExplorer.Interface.FunctionDesc;
		isShareable?: boolean;
	}
}
