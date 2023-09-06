declare module AutoRest.SdkExplorer.Interface {
	interface SchemaStore {
		/** store schemas per type so that they can be handled easily when passing through json/yaml */
		objectSchemas: { [index: string]: any };
		enumSchemas: { [index: string]: any };
		noneSchema: { [index: string]: any };
	}
}
