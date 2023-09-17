import { PlaceHolderDesc } from "./PlaceHolderDesc";
import { TypeDesc } from "./TypeDesc";

export class FunctionDesc extends PlaceHolderDesc implements AutoRest.SdkExplorer.Interface.FunctionDesc {
    type: TypeDesc;
    functionWrap: string;
    functionInvoke: string;

    constructor(data: AutoRest.SdkExplorer.Interface.FunctionDesc) {
        super(data);
        this.type = new TypeDesc(data.type!);
        this.functionInvoke = data.functionInvoke!;
        this.functionWrap = data.functionWrap!;
    }
}
