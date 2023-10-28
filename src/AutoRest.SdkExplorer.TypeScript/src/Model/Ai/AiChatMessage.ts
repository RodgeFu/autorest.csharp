import { isStringNullOrEmpty } from "../../Utils/utils";
import { AiFunctionCall } from "./AiFunctionCall";

export class AiChatMessage implements AutoRest.SdkExplorer.Interface.AiChatMessage {
    role?: string;
    content?: string;
    function_call?: AiFunctionCall;

    constructor(data: AutoRest.SdkExplorer.Interface.AiChatMessage) {
        this.role = data.role;
        this.content = data.content;
        if (!data.function_call)
            this.function_call = undefined;
        else
            this.function_call = new AiFunctionCall(data.function_call);
    }

    public static createMessageWithContent(role: "system"|"user"|"assistant", content: string): AiChatMessage {
        return new AiChatMessage({
            role: role,
            content: content
        });
    }

    public static createMessageWithFuncCall(role: "assistant", funcCall: AiFunctionCall) {
        return new AiChatMessage({
            role: "assistant",
            function_call: {
                name: funcCall.name,
                arguments: funcCall.arguments
            }
        });
    }

    public static createAskReplyWithContent(ask: string, reply: string): AiChatMessage[] {
        const arr = [];
        arr.push(this.createMessageWithContent("user", ask));
        arr.push(this.createMessageWithContent("assistant", reply))
        return arr;
    }

    public static createAskReplyWithFuncCall(ask: string, func: AiFunctionCall): AiChatMessage[] {
        const arr = [];
        arr.push(this.createMessageWithContent("user", ask));
        arr.push(this.createMessageWithFuncCall("assistant", func));
        return arr;
    }
}
