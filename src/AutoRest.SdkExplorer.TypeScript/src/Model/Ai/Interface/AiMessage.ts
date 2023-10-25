
export interface AiMessage {
    role: "system" | "user" | "assistant";
    content: string | undefined;
    function_call: {
        name: string;
        arguments: string
    } | undefined;
}
