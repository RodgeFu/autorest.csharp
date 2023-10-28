declare module AutoRest.SdkExplorer.Interface {
	interface AiChatMessage {
		/** possible value: "system", "user", "function", "tool", "assistant" */
		role?: string;
		content?: string;
		function_call?: AutoRest.SdkExplorer.Interface.AiFunctionCall;
	}
}
