declare module AutoRest.SdkExplorer.Interface {
	interface AiChatContext {
		chatMessages?: AutoRest.SdkExplorer.Interface.AiChatMessage[];
		functionPaths?: AutoRest.SdkExplorer.Interface.ApiObjectPath[];
		seed?: number;
		nucleusSamplingFactor?: number;
		presencePenalty?: number;
		frequencyPenalty?: number;
		temperature?: number;
	}
}
