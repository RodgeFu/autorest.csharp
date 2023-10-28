import { EventSlim } from "../../Utils/EventSlim";
import { isStringEqualCaseInsensitive } from "../../Utils/utils";
import { ApiDesc } from "../Code/ApiDesc";
import { CodeFormatter } from "./CodeFormatter";
import { CodeGenApiStep } from "./CodeGenApiStep";
import { CodeGenScope } from "./CodeGenScope";
import { NamespaceManager } from "./NamespaceManager";

export class CodeGenManager {

    public onPreStepAdded: EventSlim<CodeGenApiStep> = new EventSlim<CodeGenApiStep>();
    public onPreStepRemoved: EventSlim<CodeGenApiStep> = new EventSlim<CodeGenApiStep>();

    public get steps(): CodeGenApiStep[] {
        return [...this.preSteps, this.masterStep];
    }
    private _preSteps: CodeGenApiStep[];
    public get preSteps(): CodeGenApiStep[] {
        return this._preSteps;
    }

    private _masterStep: CodeGenApiStep;
    public get masterStep(): CodeGenApiStep {
        return this._masterStep;
    }

    constructor(masterStep: CodeGenApiStep, preSteps: CodeGenApiStep[] = []) {
        this._masterStep = masterStep;
        this._preSteps = preSteps;
    }

    public get hasPreSteps(): boolean { return this.preSteps.length > 0 }

    public addPreStep(stepName: string, apiDesc: ApiDesc, comment: string = ""): CodeGenApiStep {
        const n = new CodeGenApiStep(stepName, apiDesc, comment, this.masterStep);
        n.subscriptionId = this.masterStep.subscriptionId;
        n.resourceGroupName = this.masterStep.resourceGroupName;
        this._preSteps = [n, ...this.preSteps];
        this.onPreStepAdded.trigger(this, n);
        return n;
    }

    public removePreStep(stepName: string): CodeGenApiStep | undefined {
        let index = this.preSteps.findIndex(s => isStringEqualCaseInsensitive(s.stepName, stepName));
        if (index >= 0) {
            let removed = this.preSteps.splice(index, 1);
            if (removed.length === 1) {
                this.onPreStepRemoved.trigger(this, removed[0]);
                return removed[0];
            }
        }
        return undefined;
    }

    public cleanupPreStep() {
        const arr = this.preSteps;
        this._preSteps = [];
        arr.forEach(a => this.onPreStepRemoved.trigger(this, a));
    }

    public isReadyForGeneration(): boolean {
        // TODO: add more validation
        for (let step of this.steps) {
            if (!step.apiDesc.isSchemaLoaded)
                return false;
        }
        return true;
    }

    public generateCode(simplifyNamespace: boolean, formatter: CodeFormatter): string {
        if (!this.isReadyForGeneration())
            return "";
        let codePart = "";
        const scope = new CodeGenScope("global");
        if (!this.hasPreSteps) {
            codePart = this.masterStep.generatePlainCode(scope, formatter);
        }
        else {
            codePart = this.generateCodeWithPreSteps(scope, formatter);
        }

        if (codePart === "")
            return "";

        let allNamespaces = new NamespaceManager();
        allNamespaces.pushNamespaceArray(this.steps.flatMap(os => os.generateNamespaces().namespaces));

        let namespacePart = allNamespaces.toCode();

        let r = namespacePart + '\n\n' + codePart;

        if (simplifyNamespace) {
            allNamespaces.namespaces.map(v => v).reverse().forEach((ns) => {
                let nsForReg = ns.replaceAll(".", "\\.");
                // namespace.type
                let regex = new RegExp(`([^>])(global::${nsForReg})\\.([a-zA-Z0-9_]+)`, "g");
                r = r.replace(regex, `$1$3`);
            });
        }

        // current SDK CodeGen disabled nullable check, remove this after nullable check is supported in sdk codegen
        r = "#nullable disable\n" + r;

        return r;
    }

    private generateCodeWithPreSteps(scope: CodeGenScope = new CodeGenScope("global"), formatter: CodeFormatter): string {

        let armClientPart = "";
        let invokePart = "";
        let functionPart = "";
        let index = 1;
        // there should be 2 codesegment from codegen, the first is armClient and the 2nd is the sdk code
        // to keep the code simple, the first one should be shared and not use function, the second should use function with the step name as function name
        for (let os of this.steps) {
            if (armClientPart === "") {
                armClientPart = os.generateArmClientCode(scope, formatter);
            }

            const r = os.generateFunctionCode(scope, formatter, (c) => `Step ${index}: ${c}`);

            invokePart += r.invokeCode + '\n';
            functionPart += r.functionCode + '\n\n';
            index++;
        }

        return armClientPart + '\n' + invokePart + '\n' + functionPart;
    }

    public toAiPayloadAsJson(): string {
        return this.steps.map(s => s.toAiPayloadAsJson()).join("\n\n");
    }

    public toAiFunctionDefinitionAsJson(minify: boolean = false): string{
        return this.steps.map(s => s.toAiFunctionDefinitionAsJson(minify)).join("\n\n");
    }

    public toExamplePayloadAsJson(): string {
        return this.steps.map(s => s.toExamplePayloadAsJson()).join("\n\n");
    }
}
