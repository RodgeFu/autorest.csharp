import { logError } from "../../Utils/logger";
import { generateRandomFunctionName, isStringNullOrEmpty } from "../../Utils/utils";
import { AiParamDesc } from "../Ai/FunctionParameter/AiParamDesc";
import { ApiDesc } from "../Code/ApiDesc";
import { VariableDesc } from "../Code/VariableDesc";
import { ExampleDesc } from "../Example/ExampleDesc";
import { ExampleValueDesc } from "../Example/ExampleValueDesc";
import { ParamFieldBase } from "../ParamField/ParamFieldBase";
import { CodeFormatter } from "./CodeFormatter";
import { CodeGenScope } from "./CodeGenScope";
import { NamespaceManager } from "./NamespaceManager";
import { AiObjectParamDesc } from "../Ai/FunctionParameter/AiObjectParamDesc";
import { AiChatMessage } from "../Ai/AiChatMessage";
import { AiChatSession } from "../Ai/AiChatSession";
import { AiFunctionCall } from "../Ai/AiFunctionCall";
import { AiFunctionDefinition } from "../Ai/AiFunctionDefinition";
import { AzureResourceType } from "../Azure/AzureResourceType";
import { AiPayloadApplyOutput } from "../Ai/FunctionParameter/AiPayloadApplyOutput";
import { now } from "underscore";
import moment from "moment";

export class CodeGenApiStep {

    constructor(public stepName: string, public apiDesc: ApiDesc, public comment: string = "", masterStep?: CodeGenApiStep, defaultFunctionReturnVarName?: string) {
        this._aiFunctionName = apiDesc.encodedFunctionName ?? generateRandomFunctionName();
        this._masterStep = masterStep;
        this._relatedAzureResourceType = this.apiDesc.operationProviderAzureResourceType ? [this.apiDesc.operationProviderAzureResourceType] : [];
        this.resetFields();
        this.addRelatedExamples();
        this.defaultFunctionReturnVarName = defaultFunctionReturnVarName ? defaultFunctionReturnVarName : `${this.stepName.charAt(0).toLowerCase()}${this.stepName.substring(1)}_Result`;
    }

    public aiFullFunctionDefinition?: AiFunctionDefinition;
    public aiTrainMessages: AiChatMessage[] = [];
    private _aiFunctionName: string;
    public get aiFunctionName(): string {
        //return this.apiDesc.swaggerOperationId;
        return this._aiFunctionName;
    }
    public defaultFunctionReturnVarName: string;

    public get functionReturnVarName(): string {
        return this.functionReturnVarInLastGenerate?.nameToUse ?? this.defaultFunctionReturnVarName ?? "";
    }

    private _masterStep: CodeGenApiStep | undefined;
    public get masterStep(): CodeGenApiStep {
        return this._masterStep ?? this;
    }

    public get paramDepth(): number {
        let r = 0;
        this.paramFields.forEach(p => {
            let rr = p.distnaceToLeaf ?? 0;
            if (rr > r)
                r = rr;
        });
        return r;
    }

    private _functionReturnVarInLastGenerate: VariableDesc | undefined;
    public get functionReturnVarInLastGenerate(): VariableDesc | undefined {
        return this._functionReturnVarInLastGenerate;
    }

    private _paramFields: ParamFieldBase[] = [];
    public get paramFields(): ParamFieldBase[] {
        return this._paramFields;
    }

    private _relatedAzureResourceType: AzureResourceType[];
    public get relatedAzureResourceType(): AzureResourceType[] {
        return this._relatedAzureResourceType;
    }
    public addRelatedAzureResourceType(type: AzureResourceType) {
        const found = this._relatedAzureResourceType.find(t => t.toString() === type.toString());
        if (!found)
            this._relatedAzureResourceType.push(type);
    }

    public get subscriptionId(): string | undefined {
        return this.getParamFiledSubscriptionId()?.valueAsString;
    }
    public set subscriptionId(value: string | undefined) {
        const f = this.getParamFiledSubscriptionId();
        if (f)
            f.valueAsString = value ?? "";
    }

    public get resourceGroupName(): string | undefined {
        return this.getParamFiledResourceGroup()?.valueAsString;
    }
    public set resourceGroupName(value: string | undefined) {
        const f = this.getParamFiledResourceGroup();
        if (f)
            f.valueAsString = value ?? "";
    }

    private getParamFiledSubscriptionId(): ParamFieldBase | undefined {
        return this._paramFields.find(p => p.isSubscriptionIdParamField);
    }

    private getParamFiledResourceGroup(): ParamFieldBase | undefined {
        return this._paramFields.find(p => p.isResourceGroupParamField);
    }

    public generateNamespaces(): NamespaceManager {
        let ns = new NamespaceManager();
        this.paramFields.forEach(p => {
            ns.pushNamespaceArray(p.getNamespaces());
        });
        ns.pushNamespaceArray(this.apiDesc.codeSegments?.flatMap(seg => seg.usingNamespaces ?? []) ?? [])
        return ns;
    }

    public forEachParamField(func: (field: ParamFieldBase) => boolean) {
        this.paramFields.forEach(f => f.forEachInTree(func));
    }

    private generateFields() {
        return this.apiDesc.allParameters.map(p => p.createParamField(this.stepName));
    }

    public isAllFieldsDefault() {
        for (const pf of this.paramFields)
            if (pf.value !== pf.defaultValue && !pf.isSubscriptionIdParamField && !pf.isResourceGroupParamField)
                return false;
        return true;
    }

    public resetFields() {
        this._paramFields = this.generateFields();
        this.subscriptionId = this.masterStep?.subscriptionId;
        this.resourceGroupName = this.masterStep?.resourceGroupName;
        this.aiFullFunctionDefinition = this.generateFullAiFunctionDefinition();
    }

    public resetFieldsToSampleDefault() {
        this.resetFields();
        this.paramFields.forEach(p => p.resetToSampleDefault());
    }

    public resetFieldsToNotNullDefault() {
        this.resetFields();
        let hasFieldSet: boolean = true;
        while (hasFieldSet) {
            hasFieldSet = false;
            this.forEachParamField(field => {
                // add 10 as depth limitation to avoid stack stack overflow when there are circle reference
                // add more check if simple depth limitation doesnt work
                if (field.value === undefined && field.distanceToRoot < 10) {
                    field.resetToNotNullDefault();
                    hasFieldSet = true;
                }
                return true;
            })
        }
    }

    /**
     * Refresh related examples from this.apiDesc.examples if no specific examples given through parameter
     */
    public addRelatedExamples(examples?: ExampleDesc[]) {
        let exampleDescs = examples ?? this.apiDesc.examples;
        this.paramFields.forEach(p => {
            if (exampleDescs && exampleDescs.length > 0) {
                let arr: ExampleValueDesc[] = [];
                exampleDescs.forEach((exd) => {
                    exd.exampleValuesMap?.forEach((v, k) => {
                        let found = p.findMatchExample(v);
                        if (found)
                            arr.push(found);
                    });
                });
                if (arr.length > 0)
                    p.linkRelatedExamples(arr);
            }
        });
        exampleDescs.forEach(ex => {
            const fields = this.generateFields();
            ParamFieldBase.applyExample(ex, fields, "{subscription-id}", "{resource-group-name}");
            var payload = ParamFieldBase.generateAiPayload(fields);

            this.aiTrainMessages.push(
                new AiChatMessage({
                    role: "user",
                    content: `generate arguments for function '${this.aiFunctionName}' with following requirement: ${ex.exampleName ?? ex.originalFileNameWithoutExtension}`,
                    function_call: undefined
                }));
            this.aiTrainMessages.push(
                new AiChatMessage({
                    role: "assistant",
                    content: undefined,
                    function_call: {
                        name: this.aiFunctionName,
                        // double JSON.stringify to escape the json string into a string
                        arguments: AiFunctionCall.generateArgumentsString(payload)
                    }
                }));
        });
    }

    public applyExample(example: ExampleDesc) {
        ParamFieldBase.applyExample(example, this.paramFields);
    }

    public generateAiChatSession(systemMessage: string): AiChatSession | undefined {

        if (!this.aiFullFunctionDefinition)
            return undefined;
        const messages: AiChatMessage[] = [new AiChatMessage({
            role: "system",
            content: systemMessage
        })];
        messages.push(...this.aiTrainMessages);

        const session: AiChatSession = new AiChatSession({
            sessionId: undefined,
            chatMessages: messages,
            functions: [this.aiFullFunctionDefinition]
        });
        return session;
    }

    public generateAiPayloadForExample(example: ExampleDesc): { [index: string]: any } {
        const fields = this.generateFields();
        ParamFieldBase.applyExample(example, fields);
        return ParamFieldBase.generateAiPayload(fields);
    }

    public applyAiPayload(payload: any, output : AiPayloadApplyOutput) {
        return ParamFieldBase.applyAiPayload(this.paramFields, payload, output);
    }

    public generateAiFunctionCall() {
        return AiFunctionCall.createAiFunctionCall(this.aiFunctionName, this.generateAiPayload());
    }

    public generateAiPayload(): { [index: string]: any } {
        return ParamFieldBase.generateAiPayload(this.paramFields);
    }

    private generateFullAiFunctionDefinition(): AiFunctionDefinition {
        const fields = this.generateFields();
        fields.forEach(p => p.resetToSampleDefault());
        return ParamFieldBase.generateAiFunctionDefinition(this.aiFunctionName, `definition to trigger Azure SDK function ${this.aiFunctionName}. In more detail, ${this.apiDesc.apiDescription}.`, fields);
    }

    public generateAiFunctionDefinition(): AiFunctionDefinition {
        return ParamFieldBase.generateAiFunctionDefinition(this.aiFunctionName, `definition to trigger Azure SDK function ${this.aiFunctionName}. In more detail, ${this.apiDesc.apiDescription}.`, this.paramFields);
    }

    public toAiPayloadAsJson(minify: boolean = false): string {
        return JSON.stringify(this.generateAiPayload(), undefined, minify ? undefined : "  ");
    }

    public toAiFunctionDefinitionAsJson(minify: boolean = false): string {
        return this.generateAiFunctionDefinition().toJsonForOpenAi(minify);
    }

    public generateExampleDesc(): ExampleDesc {

        let r: ExampleDesc = new ExampleDesc({
            exampleName: "LiveExampleDesc",
            embeddingText: undefined,
            embeddingVector: undefined,
            explorerCodeGenVersion: this.masterStep.apiDesc.explorerCodeGenVersion,
            generatedTimestamp: undefined,
            language: this.masterStep.apiDesc.language,
            operationName: this.masterStep.apiDesc.operationName,
            originalFilePath: "",
            originalFileNameWithoutExtension: "",
            resourceName: this.masterStep.apiDesc.resourceName,
            serviceName: this.masterStep.apiDesc.serviceName,
            sdkOperationId: this.masterStep.apiDesc.sdkOperationId,
            sdkFullUniqueName: this.masterStep.apiDesc.fullUniqueName,
            sdkPackageName: this.masterStep.apiDesc.sdkPackageName,
            sdkPackageVersion: this.masterStep.apiDesc.sdkPackageVersion,
            swaggerOperationId: this.masterStep.apiDesc.swaggerOperationId,
            exampleValues: undefined
        });
        r.exampleValuesMap = new Map<string, ExampleValueDesc>();
        this.paramFields.forEach(paramField => {
            const ev = paramField.generateExampleValue(r);
            if(ev)
                r.exampleValuesMap?.set(paramField.fieldName, ev);
        });
        return r;
    }

    public toExamplePayloadAsJson(): string {
        return this.generateExampleDesc().toJson();
    }

    public applyExampleByName(exampleName: string) {
        const ex = this.apiDesc.examples?.find(v => v.exampleName === exampleName);
        if (ex)
            this.applyExample(ex);
    }

    public generatePlainCode(scope: CodeGenScope = new CodeGenScope("global"), formatter: CodeFormatter) {

        let codeSeg: string[] = [];
        this.apiDesc.codeSegments?.forEach(seg => {
            codeSeg.push(seg.generatePlainCode(scope, this.paramFields, formatter) ?? '');
        })

        let codePart = codeSeg.join('\n');
        return codePart;
    }

    // there should be 2 codesegment from codegen, the first is armClient and the 2nd is the sdk code, which is the only scenario now
    // so to keep the code simple, we assume these segment in the code gen now
    // TODO: update this part of codegen if we have more scenario from CodeSegment
    public generateArmClientCode(scope: CodeGenScope = new CodeGenScope("global"), formatter: CodeFormatter): string {
        const GET_ARMCLIENT_SEGMENT = "GET_ARM_CLIENT";
        let segs = this.apiDesc.codeSegments;
        if (segs?.length !== 2) {
            console.error("Unexpected CodeSegment length: " + segs?.length ?? 0);
            return "";
        }
        if (segs[0].key !== GET_ARMCLIENT_SEGMENT) {
            console.error("Unexpected first Segment: " + segs[0].key);
            return "";
        }

        let armClientSeg = segs[0];

        const armClientPart = armClientSeg.generatePlainCode(scope, this.paramFields, formatter);
        return armClientPart;
    }

    // there should be 2 codesegment from codegen, the first is armClient and the 2nd is the sdk code, which is the only scenario now
    // so to keep the code simple, we assume these segment in the code gen now
    // TODO: update this part of codegen if we have more scenario from CodeSegment
    public generateFunctionCode(scope: CodeGenScope = new CodeGenScope("global"),
        formatter: CodeFormatter,
        getFuncComment: (comment: string) => string = comment => comment): { invokeCode: string, functionCode: string } {
        const GET_ARMCLIENT_SEGMENT = "GET_ARM_CLIENT";

        let segs = this.apiDesc.codeSegments;
        if (segs?.length !== 2) {
            logError("Unexpected CodeSegment length: " + segs?.length ?? 0);
            return { invokeCode: "", functionCode: "" };
        }
        if (segs[0].key !== GET_ARMCLIENT_SEGMENT) {
            logError("Unexpected first Segment: " + segs[0].key);
            return { invokeCode: "", functionCode: "" };
        }
        if (segs[1].outputResult && segs[1].outputResult?.length > 1) {
            logError("more than one output result for segment: " + segs[1].key);
            return { invokeCode: "", functionCode: "" };
        }

        let codeSeg = segs[1];

        let c = getFuncComment(this.comment);
        c = isStringNullOrEmpty(c) ? "" : `// ${c}\n`;
        const invokePart = codeSeg.generateCodeAsInvokeFunction(this.stepName, scope, this.defaultFunctionReturnVarName, formatter);
        const invokeCode = c + invokePart.code;
        this._functionReturnVarInLastGenerate = invokePart.returnVar;

        const functionCode = codeSeg.generateCodeAsFunction(this.stepName, this.paramFields, scope, formatter);


        return { invokeCode: invokeCode, functionCode: functionCode }
    }
}
