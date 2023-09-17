declare module AutoRest.SdkExplorer.Interface {
	interface AzureResourceIdentifier {
		/** save the raw id only for readability and troubleshooting purpose */
		rawId?: string;
		/** segments and action are source of truth */
		resourceSegments?: AutoRest.SdkExplorer.Interface.AzureResourceIdentifierSegment[];
		action?: string;
	}
}
