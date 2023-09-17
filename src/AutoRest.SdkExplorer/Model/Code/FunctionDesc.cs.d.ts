declare module AutoRest.SdkExplorer.Interface {
	interface FunctionDesc extends PlaceHolderDesc {
		type?: AutoRest.SdkExplorer.Interface.TypeDesc;
		functionWrap?: string;
		functionInvoke?: string;
	}
}
