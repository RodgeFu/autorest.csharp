declare module AutoRest.SdkExplorer.Interface {
	interface AiFunctionDefinition {
		name?: string;
		description?: string;
		/** something like:{\"name\":\"search_hotels\",\"description\":\"Retrieves hotels from the search index based on the parameters provided\",\"parameters\":{\"type\":\"object\",\"properties\":{\"location\":{\"type\":\"string\",\"description\":\"The location of the hotel (i.e. Seattle, WA)\"},\"max_price\":{\"type\":\"number\",\"description\":\"The maximum price for the hotel\"},\"features\":{\"type\":\"string\",\"description\":\"A comma separated list of features (i.e. beachfront, free wifi, etc.)\"}},\"required\":[\"location\"]}} */
		parametersAsString?: string;
	}
}
