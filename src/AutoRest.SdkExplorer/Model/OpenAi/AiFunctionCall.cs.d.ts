declare module AutoRest.SdkExplorer.Interface {
	interface AiFunctionCall {
		name?: string;
		/** something like:"{\n  \"location\": \"San Diego\",\n  \"max_price\": 300,\n  \"features\": \"beachfront,free breakfast\"\n}" */
		arguments?: string;
	}
}
