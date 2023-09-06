declare module AutoRest.SdkExplorer.Interface {
	interface FunctionDesc {
		key: string;
		suggestedName: string;
		type: AutoRest.SdkExplorer.Interface.TypeDesc;
		functionWrap: string;
		functionInvoke: string;
	}
}
