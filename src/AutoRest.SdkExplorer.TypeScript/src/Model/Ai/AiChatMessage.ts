import { tryJsonToObj } from "../../Utils/utils";
import { AiFunctionCall } from "./AiFunctionCall";

export type AiChatRole = "system" | "user" | "assistant" | "none";

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

    public getAiPayloadApiName() : string | undefined {
        return this.function_call?.name;
    }

    public getAiPayload(checkJsonInContent: boolean = false): any {
        const argObj = this.function_call?.argumentsAsObject;
        if (argObj)
            return argObj;

        const tContent = this.content?.trim();
        let r = tryJsonToObj(tContent);
        if (r)
            return r;

        //// if content only contains json payload, consider it as func_call.arguments too
        //if (tContent && tContent.startsWith("{") && tContent.endsWith("}")) {
        //    const r = jsonStringToObj(tContent)
        //    if (r !== undefined)
        //        return r;
        //    if (tContent.indexOf("\\\"") >= 0) {
        //        const r = jsonEscapedStringToObj(tContent);
        //        if (r !== undefined)
        //            return r;
        //    }
        //}

        // make sure payload in the first group;
        let embedFormats = [
            /```json\s*({.+?})\s*```/gis,
            /```javascript\s*({.+?})\s*```/gis,
            /```\s*({.+?})\s*```/gis,
            /\s*({.*subscriptionId.*})\s*/gis
        ];

        if (checkJsonInContent && this.content) {
            const content = this.content;

            for (let e of embedFormats) {
                let matches = content.matchAll(e);
                for (let m of matches) {
                    let payload = m[1];
                    try {
                        let r = tryJsonToObj(payload);
                        if (r)
                            return r;
                    }
                    catch { }
                }
                
            }
        }

        //const START_MARK = "```json";
        //const END_MARK = "```";
        //if (checkJsonInContent && this.content) {
        //    const content = this.content;
        //    const start = content.indexOf(START_MARK);
        //    if (start >= 0) {
        //        const end = content.indexOf(END_MARK, start + START_MARK.length);
        //        if (end >= 0) {
        //            const jsonPayload = content.substring(start + START_MARK.length, end);
        //            return JSON.parse(jsonPayload);
        //        }
        //    }
        //}


        return undefined;
    }

    private extractAiPayload(content: string, startMark: string, endMark: string) {

    }

    public static createMessageWithContent(role: AiChatRole, content: string): AiChatMessage {
        return new AiChatMessage({
            role: role,
            content: content
        });
    }

    public static createMessageWithFuncCall(role: "assistant", funcCall: AiFunctionCall) {
        return new AiChatMessage({
            role: role,
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
