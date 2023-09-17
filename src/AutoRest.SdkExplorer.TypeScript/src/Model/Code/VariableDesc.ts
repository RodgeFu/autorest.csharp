import { PlaceHolderDesc } from "./PlaceHolderDesc";
import { TypeDesc } from "./TypeDesc";

export class VariableDesc extends PlaceHolderDesc implements AutoRest.SdkExplorer.Interface.VariableDesc {
    type: TypeDesc;

    constructor(data: AutoRest.SdkExplorer.Interface.VariableDesc) {
        super(data);
        this.type = new TypeDesc(data.type!);
    }
}
