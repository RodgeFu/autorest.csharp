import { convertStringIndexArrayFromMap, convertStringIndexdArray, convertStringIndexdArrayToMap, getStringIndexArrayFromMap } from "../../Utils/utils";
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

    get exampleValues(): { [index: string]: ExampleValueDesc } | undefined {
        return getStringIndexArrayFromMap(this.exampleValuesMap);
    }

    exampleValuesMap?: Map<string, ExampleValueDesc>;

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

        this.exampleValuesMap = convertStringIndexdArrayToMap(data.exampleValues, (d, k) => new ExampleValueDesc(d, this, k));
    }

    public setExampleValues(values: Map<string, ExampleValueDesc>) {
        this.exampleValuesMap = new Map<string, ExampleValueDesc>();
        values.forEach((v, k) => {
            this.exampleValuesMap!.set(k, v);
        })
    }

    public toPayload(): AutoRest.SdkExplorer.Interface.ExampleDesc {
        const r: AutoRest.SdkExplorer.Interface.ExampleDesc = {
            language: this.language,
            sdkPackageName: this.sdkPackageName,
            sdkPackageVersion: this.sdkPackageVersion,
            explorerCodeGenVersion: this.explorerCodeGenVersion,
            generatedTimestamp: this.generatedTimestamp,
            serviceName: this.serviceName,
            resourceName: this.resourceName,
            operationName: this.operationName,
            swaggerOperationId: this.swaggerOperationId,
            sdkOperationId: this.sdkOperationId,
            exampleName: this.exampleName,
            originalFilePath: this.originalFilePath,
            originalFileNameWithoutExtension: this.originalFileNameWithoutExtension,
            embeddingText: this.embeddingText,
            embeddingVector: this.embeddingVector,
            exampleValues: this.exampleValuesMap === undefined ? undefined : convertStringIndexArrayFromMap(this.exampleValuesMap, (v, k) => v.toPayload())
        };
        return r;
    }

    public toJson(): string {
        return JSON.stringify(this.toPayload(), (key: string, value: any) => {
            const toIgnore = [
                "serializerName",
                "modelName",
            ];
            if (toIgnore.includes(key))
                return undefined;
            return value;
        }, "  ");
    }
}
