import { isStringEqualCaseInsensitive } from "../../Utils/utils";
import { AzureResourceType } from "../Azure/AzureResourceType";
import { ExampleDesc } from "../Example/ExampleDesc";
import { SchemaStore } from "../Schema/SchemaStore";
import { CodeSegmentDesc } from "./CodeSegmentDesc";
import { ParameterDesc } from "./ParameterDesc";

export class ApiDesc implements AutoRest.SdkExplorer.Interface.ApiDesc {
    language!: string;
    sdkPackageName!: string;
    sdkPackageVersion!: string;
    explorerCodeGenVersion!: string;
    generatedTimestamp!: string;
    dependencies!: string[];
    serviceName!: string;
    resourceName!: string;
    operationName!: string;
    swaggerOperationId!: string;
    sdkOperationId!: string;
    description!: string;
    apiDescription!: string;
    aiDescription!: string;
    fullUniqueName!: string;
    encodedFunctionName!: string;
    operationNameWithParameters!: string;
    operationNameWithScopeAndParameters!: string;
    operationProviderAzureResourceType?: AzureResourceType;
    operationProviderType?: string;
    apiVersion!: string;
    requestPath!: string;
    operationMethodParameters: ParameterDesc[] = [];
    codeSegments: CodeSegmentDesc[] = [];
    schemaStore?: SchemaStore;
    embeddingText?: string;
    embeddingVector?: string;


    private _examples: ExampleDesc[] = [];
    get exampleInitialized(): boolean { return this._examples.length > 0; }
    get examples(): ExampleDesc[] { return this._examples; }

    private _isSchemaLoaded: boolean = false;
    get isSchemaLoaded(): boolean { return this._isSchemaLoaded; }

    constructor(data: AutoRest.SdkExplorer.Interface.ApiDesc) {
        this.refresh(data);
    }

    public refresh(data: AutoRest.SdkExplorer.Interface.ApiDesc) {
        this.language = data.language!;
        this.sdkPackageName = data.sdkPackageName!;
        this.sdkPackageVersion = data.sdkPackageVersion!;
        this.explorerCodeGenVersion = data.explorerCodeGenVersion!;
        this.generatedTimestamp = data.generatedTimestamp!;
        this.dependencies = data.dependencies!;
        this.serviceName = data.serviceName!;
        this.resourceName = data.resourceName!;
        this.operationName = data.operationName!;
        this.swaggerOperationId = data.swaggerOperationId!;
        this.sdkOperationId = data.sdkOperationId!;
        this.description = data.description!;
        this.apiDescription = data.apiDescription ?? "";
        this.aiDescription = data.aiDescription ?? "";
        this.fullUniqueName = data.fullUniqueName!;
        this.encodedFunctionName = data.encodedFunctionName!;
        this.operationNameWithParameters = data.operationNameWithParameters!;
        this.operationNameWithScopeAndParameters = data.operationNameWithScopeAndParameters!;
        this.operationProviderAzureResourceType = data.operationProviderAzureResourceType ? new AzureResourceType(data.operationProviderAzureResourceType) : undefined;
        this.operationProviderType = data.operationProviderType;
        this.apiVersion = data.apiVersion!;
        this.requestPath = data.requestPath!;
        this.operationMethodParameters = data.operationMethodParameters?.map(d => new ParameterDesc(d)) ?? [];
        this.codeSegments = data.codeSegments?.map(d => new CodeSegmentDesc(d)) ?? [];
        this.schemaStore = data.schemaStore ? new SchemaStore(data.schemaStore) : undefined;
        this.embeddingText = data.embeddingText;
        this.embeddingVector = data.embeddingVector;
        this.loadSchema();
    }

    private loadSchema() {
        const ss = this.schemaStore;
        if (ss && ss.schemaCount > 0) {
            this.operationMethodParameters?.forEach(p => p.loadSchema(ss));
            this.allParameters.forEach(p => p.loadSchema(ss));
            this._isSchemaLoaded = true;
        }
    }

    public get allParameters(): ParameterDesc[] {
        return this.codeSegments?.flatMap((v) => {
            let r: ParameterDesc[] = v.parameters ?? [];
            return r;
        }) ?? [];
    }

    public get isCreateOperation(): boolean {
        return isStringEqualCaseInsensitive(this.operationName, "createorupdate") ||
            isStringEqualCaseInsensitive(this.operationName, "create");
    }

    public get isGetOperation(): boolean {
        return isStringEqualCaseInsensitive(this.operationName, "get");
    }
}
