import { ApiObjectPath } from "../Code/ApiObjectPath";
import { AiChatMessage } from "./AiChatMessage";


export class AiChatResult implements AutoRest.SdkExplorer.Interface.AiChatResult {

    functionPath?: ApiObjectPath;
    message?: AiChatMessage;

    constructor(data: AutoRest.SdkExplorer.Interface.AiChatResult) {
        if(data.message)
            this.message = new AiChatMessage(data.message);
        if (data.functionPath)
            this.functionPath = new ApiObjectPath(data.functionPath);
    }
}
