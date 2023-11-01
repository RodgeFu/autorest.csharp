declare module AutoRest.SdkExplorer.Interface {
	interface AiChatSession {
		sessionId?: string;
		chatMessages?: AutoRest.SdkExplorer.Interface.AiChatMessage[];
		functions?: AutoRest.SdkExplorer.Interface.AiFunctionDefinition[];
		nucleusSamplingFactor?: number;
		presencePenalty?: number;
		frequencyPenalty?: number;
		temperature?: number;
	}
}
