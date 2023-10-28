
export class AiFunctionCall implements AutoRest.SdkExplorer.Interface.AiFunctionCall {
    name?: string;
    /** something like:"{\n  \"location\": \"San Diego\",\n  \"max_price\": 300,\n  \"features\": \"beachfront,free breakfast\"\n}" */
    arguments?: string;

    constructor(data: AutoRest.SdkExplorer.Interface.AiFunctionCall) {
        this.name = data.name;
        this.arguments = data.arguments;
    }

    public get argumentsAsObject(): any {
        if (!this.arguments)
            return undefined;
        try {
            var obj = JSON.parse(this.arguments);
            return obj;
        }
        catch
        {
            try {
                var jsonString = JSON.parse(`"${this.arguments}"`);
                var obj = JSON.parse(jsonString);
                return obj;
            }
            catch {
                console.warn("unexpected error when converting function_call arguement from openAI: \n" + this.arguments);
                return undefined;
            }
        }
    }

    public toReadableArgument() : string | undefined {
        if (!this.arguments)
            return undefined;
        if (!this.argumentsAsObject)
            return this.arguments;
        try {
            return JSON.stringify(this.argumentsAsObject, undefined, "  ");
        }
        catch {
            console.warn("unexpected function_call arg from openAI: \n" + this.arguments);
            return this.arguments;
        }
    }

    public static createAiFunctionCall(name: string, argObj: any) {
        return new AiFunctionCall({
            name: name,
            arguments: AiFunctionCall.generateArgumentsString(argObj)
        });
    }

    public static generateArgumentsString(obj: any) : string {
        if (!obj)
            return "";
        return JSON.stringify(JSON.stringify(obj)).slice(1, -1);
    }
}
