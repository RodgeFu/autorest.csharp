import { AiChatMessage } from "./AiChatMessage";
import { AiFunctionDefinition } from "./AiFunctionDefinition";

export class AiChatSession implements AutoRest.SdkExplorer.Interface.AiChatSession {
    sessionId?: string;
    chatMessages?: AiChatMessage[];
    functions?: AiFunctionDefinition[];

    nucleusSamplingFactor? : number;
    presencePenalty? : number;
    frequencyPenalty?: number;
    temperature?: number;

    constructor(data: AutoRest.SdkExplorer.Interface.AiChatSession) {
        this.sessionId = data.sessionId;
        this.chatMessages = data.chatMessages?.map(m => new AiChatMessage(m));
        this.functions = data.functions?.map(f => new AiFunctionDefinition({
            name: f.name,
            description: f.description,
            parametersAsString: f.parametersAsString
        }));
    }
}
