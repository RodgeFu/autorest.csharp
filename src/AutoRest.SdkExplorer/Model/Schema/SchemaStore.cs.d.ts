declare module AutoRest.SdkExplorer.Interface {
	interface SchemaStore {
		/** store schemas per type so that they can be handled easily when passing through json/yaml */
		objectSchemas?: { [index: string]: AutoRest.SdkExplorer.Interface.SchemaObject };
		enumSchemas?: { [index: string]: AutoRest.SdkExplorer.Interface.SchemaEnum };
		noneSchemas?: { [index: string]: AutoRest.SdkExplorer.Interface.SchemaNone };
	}
}
