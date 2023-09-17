import { convertStringIndexdArray, convertStringIndexdArrayToMap } from "../../Utils/utils";
import { ExampleValueDesc } from "./ExampleValueDesc";

export class ExampleDesc implements AutoRest.SdkExplorer.Interface.ExampleDesc {
    language: string;
    sdkPackageName: string;
    sdkPackageVersion: string;
    explorerCodeGenVersion: string;
    generatedTimestamp: string;
    serviceName: string;
    resourceName: string;
    operationName: string;
    swaggerOperationId: string;
    sdkOperationId: string;
    exampleName: string;
    originalFilePath: string;
    originalFileNameWithoutExtension: string;
    embeddingText: string;
    embeddingVector: string;
    exampleValues?: { [index: string]: ExampleValueDesc }

    exampleValuesMap: Map<string, ExampleValueDesc>;

    constructor(data: AutoRest.SdkExplorer.Interface.ExampleDesc) {
        this.language = data.language!;
        this.sdkPackageName = data.sdkPackageName!;
        this.sdkPackageVersion = data.sdkPackageVersion!;
        this.explorerCodeGenVersion = data.explorerCodeGenVersion!;
        this.generatedTimestamp = data.generatedTimestamp!;
        this.serviceName = data.serviceName!;
        this.resourceName = data.resourceName!;
        this.operationName = data.operationName!;
        this.swaggerOperationId = data.swaggerOperationId!;
        this.sdkOperationId = data.sdkOperationId!;
        this.exampleName = data.exampleName!;
        this.originalFilePath = data.originalFilePath!;
        this.originalFileNameWithoutExtension = data.originalFileNameWithoutExtension!;
        this.embeddingText = data.embeddingText!;
        this.embeddingVector = data.embeddingVector!;
        this.exampleValues = convertStringIndexdArray(data.exampleValues, (d, k) => new ExampleValueDesc(d, this, k));

        this.exampleValuesMap = convertStringIndexdArrayToMap(data.exampleValues, (d, k) => new ExampleValueDesc(d, this, k));
    }
}
