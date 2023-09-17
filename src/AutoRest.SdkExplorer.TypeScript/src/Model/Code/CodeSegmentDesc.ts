import { logError, logWarning } from "../../Utils/logger";
import { CodeFormatter } from "../CodeGen/CodeFormatter";
import { CodeGenScope } from "../CodeGen/CodeGenScope";
import { ParamFieldBase } from "../ParamField/ParamFieldBase";
import { FunctionDesc } from "./FunctionDesc";
import { ParameterDesc } from "./ParameterDesc";
import { PlaceHolderDesc } from "./PlaceHolderDesc";
import { VariableDesc } from "./VariableDesc";

export class CodeSegmentDesc extends PlaceHolderDesc implements AutoRest.SdkExplorer.Interface.CodeSegmentDesc {
    code: string;
    usingNamespaces: string[];
    dependencies: VariableDesc[];
    outputResult: VariableDesc[];
    variables: VariableDesc[];
    parameters: ParameterDesc[];
    function?: FunctionDesc;
    isShareable: boolean;


    constructor(data: AutoRest.SdkExplorer.Interface.CodeSegmentDesc) {
        super(data);
        this.code = data.code ?? "";
        this.usingNamespaces = data.usingNamespaces ?? [];
        this.dependencies = data.dependencies?.map(d => new VariableDesc(d)) ?? [];
        this.outputResult = data.outputResult?.map(d => new VariableDesc(d)) ?? [];
        this.variables = data.variables?.map(d => new VariableDesc(d)) ?? [];
        this.parameters = data.parameters?.map(d => new ParameterDesc(d)) ?? [];
        this.function = data.function ? new FunctionDesc(data.function) : undefined;
        this.isShareable = data.isShareable ?? false;
    }

    public generatePlainCode(scope: CodeGenScope, fields: ParamFieldBase[], formatter: CodeFormatter): string {

        // for plain code, we only need to link to the depended var
        this.dependencies?.forEach(dep => {
            scope.mergeToVariable(dep, true);
        })

        this.variables?.forEach(v => {
            scope.pushVariable(v);
        })

        this.outputResult?.forEach(v => {
            scope.mergeToVariable(v, false);
        })

        let newCode = this.code!;
        this.dependencies?.forEach(v => {
            if (v.key && v.nameToUse)
                newCode = newCode?.replaceAll(v.key, v.nameToUse);
        })
        this.variables?.forEach(v => {
            if (v.key && v.nameToUse)
                newCode = newCode?.replaceAll(v.key, v.nameToUse);
        })

        this.parameters?.forEach(p => {
            if (p.key) {
                let found = fields.find(pf => pf.fieldName === p.suggestedName)
                if (found) {
                    let code = found.getValueForCode(""/*indent*/, formatter)
                    newCode = newCode?.replaceAll(p.key, code);
                }
            }
        })

        return newCode;
    }

    public generateCodeAsFunction(functionName: string, fields: ParamFieldBase[], parentScope: CodeGenScope | undefined, formatter: CodeFormatter): string {
        if (!this.function) {
            logError("No Function defined for the codesegment: " + this.suggestedName);
            return "";
        }
        // dont need parent scope in function
        let funcScope = new CodeGenScope(this.function.suggestedName, parentScope);

        // all dependencies should be in the function parameters as local variable
        this.dependencies?.forEach(dep => {
            funcScope.pushVariable(dep);
        })

        this.variables?.forEach(v => {
            funcScope.pushVariable(v);
        })

        this.outputResult?.forEach(v => {
            funcScope.mergeToVariable(v, false);
        })

        let newCode: string = this.code!;

        this.parameters?.forEach(p => {
            if (p.key) {
                let found = fields.find(pf => pf.fieldName === p.suggestedName)
                if (found) {
                    let code = found.getValueForCode("", formatter)
                    newCode = newCode?.replaceAll(p.key, code);
                }
            }
        })

        newCode = this.function.functionWrap.replaceAll(/^( *)__FUNC_CODESEGMENT_CODE__/gm, (substring: string, indent: string) => {
            return indent + newCode.replaceAll("\n", "\n" + indent);
        });

        this.dependencies?.forEach(v => {
            if (v.key && v.nameToUse)
                newCode = newCode?.replaceAll(v.key, v.nameToUse);
        })
        this.variables?.forEach(v => {
            if (v.key && v.nameToUse)
                newCode = newCode?.replaceAll(v.key, v.nameToUse);
        })

        newCode = newCode.replaceAll(this.function.key, functionName);

        return newCode;
    }

    public generateCodeAsInvokeFunction(functionName: string, scope: CodeGenScope, returnVarName: string, formatter: CodeFormatter): { code: string, returnVar: VariableDesc | undefined } {
        if (!this.function) {
            logError("No Function defined for the codesegment: " + this.suggestedName);
            return { code: "", returnVar: undefined };
        }

        // when invoke function, we only need to link to the depended var
        this.dependencies?.forEach(dep => {
            scope.mergeToVariable(dep, true);
        })

        let r = this.function.functionInvoke.replaceAll(this.function.key, functionName);

        let returnVar: VariableDesc | undefined;
        if (this.outputResult && this.outputResult.length === 1) {
            returnVar = new VariableDesc({
                key: this.outputResult[0].key,
                suggestedName: returnVarName,
                type: this.outputResult[0].type
            });
            scope.pushVariable(returnVar);
            r = r.replaceAll(returnVar.key!, returnVar.nameToUse!);
        }

        this.dependencies?.forEach(v => {
            if (v.key && v.nameToUse)
                r = r?.replaceAll(v.key, v.nameToUse);
        })

        return {
            code: r + ";",
            returnVar: returnVar
        };
    }
}
