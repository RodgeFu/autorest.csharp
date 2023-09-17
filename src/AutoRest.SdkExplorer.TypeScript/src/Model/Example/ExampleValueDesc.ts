import { convertStringIndexdArray, convertStringIndexdArrayToMap } from "../../Utils/utils";
import { ExampleDesc } from "./ExampleDesc";

export class ExampleValueDesc implements AutoRest.SdkExplorer.Interface.ExampleValueDesc {
    serializerName: string;
    modelName: string;
    schemaType: string;
    cSharpName: string;
    rawValue?: string;
    propertyValues?: { [index: string]: ExampleValueDesc };
    arrayValues?: ExampleValueDesc[];

    propertyValuesMap: Map<string, ExampleValueDesc>;

    /**
     * 
     * @param data
     * @param ownerExample - the example this exampleValue belongs to
     * @param fieldName - the name of the example belongs to, usually, it's the parameter name for the top level parameter or the property name if it's in the schema structure
     */
    public constructor(data: AutoRest.SdkExplorer.Interface.ExampleValueDesc, public ownerExample: ExampleDesc, public fieldName: string) {
        this.serializerName = data.serializerName!;
        this.modelName = data.modelName!;
        this.schemaType = data.schemaType!;
        this.cSharpName = data.cSharpName!;
        this.rawValue = data.rawValue;
        this.propertyValues = convertStringIndexdArray(data.propertyValues, (d, k) => new ExampleValueDesc(d, ownerExample, fieldName));
        this.arrayValues = data.arrayValues ? data.arrayValues.map(d => new ExampleValueDesc(d, ownerExample, fieldName)) : undefined;

        this.propertyValuesMap = convertStringIndexdArrayToMap(data.propertyValues, d => new ExampleValueDesc(d, ownerExample, fieldName));
    }
}
