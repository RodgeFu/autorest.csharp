import { values } from "underscore";
import { logError } from "../../Utils/logger";
import { isStringNullOrEmpty } from "../../Utils/utils";
import { ApiDesc } from "../Code/ApiDesc";
import { VariableDesc } from "../Code/VariableDesc";
import { ExampleDesc } from "../Example/ExampleDesc";
import { ExampleValueDesc } from "../Example/ExampleValueDesc";
import { ParamFieldBase } from "../ParamField/ParamFieldBase";
import { CodeGenScope } from "./CodeGenScope";
import { NamespaceManager } from "./NamespaceManager";
import { CodeFormatter } from "./CodeFormatter";
import { format } from "node:path/win32";

export class CodeGenApiStep {

    constructor(public stepName: string, public apiDesc: ApiDesc, public comment: string = "", masterStep?: CodeGenApiStep, defaultFunctionReturnVarName? : string) {
        this._masterStep = masterStep;
        this.resetFields();
        this.addRelatedExamples();
        this.defaultFunctionReturnVarName = defaultFunctionReturnVarName ? defaultFunctionReturnVarName : `${this.stepName.charAt(0).toLowerCase()}${this.stepName.substring(1)}_Result`;
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

    public resetFields() {
        this._paramFields = this.apiDesc.allParameters.map(p => p.createParamField(this.stepName));
        this.subscriptionId = this.masterStep?.subscriptionId;
        this.resourceGroupName = this.masterStep?.resourceGroupName;
    }

    /**
     * Refresh related examples from this.apiDesc.examples if no specific examples given through parameter
     */
    public addRelatedExamples(examples?: ExampleDesc[]) {
        this.paramFields.forEach(p => {
            let exampleDescs = examples ?? this.apiDesc.examples;
            if (exampleDescs && exampleDescs.length > 0) {
                let arr: ExampleValueDesc[] = [];
                exampleDescs.forEach((exd) => {
                    exd.exampleValuesMap.forEach((v, k) => {
                        let found = p.findMatchExample(v);
                        if (found)
                            arr.push(found);
                    });
                });
                if (arr.length > 0)
                    p.linkRelatedExamples(arr);
            }
        })
    }

    public applyExample(example: ExampleDesc) {
        let s: Set<string> = new Set<string>();
        example.exampleValuesMap.forEach((value, key) => {
            let foundExampleValue: ExampleValueDesc | undefined = undefined;
            let p = this.paramFields.find(pp => {
                foundExampleValue = pp.findMatchExample(value);
                return foundExampleValue !== undefined;
            });
            if (p !== undefined && foundExampleValue &&
                // not load example value for subscriptionId and resourceGroup because
                // 1. it can't be right, 2. user has to reset them again if they have been set
                (!p.isSubscriptionIdParamField) &&
                (!p.isResourceGroupParamField)) {
                s.add(p.fieldName!);
                p.setExampleValue(foundExampleValue);
            }
            else {
                console.warn("can't find field for example: " + key);
            }
        })

        this.paramFields.forEach(v => {
            if ((!v.isSubscriptionIdParamField) &&
                (!v.isResourceGroupParamField) &&
                (!s.has(v.fieldName!)))
                v.resetToIgnoreOrDefault();
        })
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
