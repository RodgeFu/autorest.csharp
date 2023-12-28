import { ApiObjectPath } from "../Code/ApiObjectPath";
import { AiChatMessage } from "./AiChatMessage";

export class AiChatContext implements AutoRest.SdkExplorer.Interface.AiChatContext {
    chatMessages?: AiChatMessage[];
    functionPaths?: ApiObjectPath[];

    seed?: number;
    nucleusSamplingFactor?: number;
    presencePenalty?: number;
    frequencyPenalty?: number;
    temperature?: number;

    constructor(data: AutoRest.SdkExplorer.Interface.AiChatContext) {
        this.chatMessages = data.chatMessages?.map(m => new AiChatMessage(m));
        this.functionPaths = data.functionPaths?.map(p => new ApiObjectPath(p));
        this.seed = data.seed;
        this.nucleusSamplingFactor = data.nucleusSamplingFactor;
        this.presencePenalty = data.presencePenalty;
        this.frequencyPenalty = data.frequencyPenalty;
        this.temperature = data.temperature;

    }
}
