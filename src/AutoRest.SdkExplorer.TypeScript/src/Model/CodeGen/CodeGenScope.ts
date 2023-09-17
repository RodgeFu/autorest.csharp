import { VariableDesc } from "../Code/VariableDesc";

export class CodeGenScope {

    private _variables: VariableDesc[] = [];

    public get allDefinedVariables(): VariableDesc[] {
        let scopeVariablesArray = [];
        let ps: CodeGenScope | undefined = this;
        while (ps) {
            scopeVariablesArray.push(ps._variables);
            ps = ps.parentScope;
        }
        return scopeVariablesArray.flat();
    }

    constructor(public scopeName: string, public parentScope?: CodeGenScope) {

    }

    public pushVariable(variable: VariableDesc) {
        this.checkAndSetNameToUseForVariable(variable);
        this._variables.push(variable);
    }

    public findVariable(key: string, includeParentScope: boolean = true) {
        let r = this._variables.find(v => v.key === key);
        if (!r && includeParentScope)
            r = this.allDefinedVariables.find(v => v.key === key)
        return r;
    }

    public mergeToVariable(variable: VariableDesc, includeParentScope: boolean = true) {
        let r = this.findVariable(variable.key!, includeParentScope);
        if (!r)
            throw `no variable found to merge for key=${variable.key}, suggestedName=${variable.suggestedName}`;
        variable.nameToUse = r.nameToUse;
    }

    public createSubScope(scopeName: string) {
        return new CodeGenScope(scopeName, this);
    }

    private checkAndSetNameToUseForVariable(varToCheck: VariableDesc) {
        let nameToUse = this.checkAndGetNameToUse(varToCheck.suggestedName!);
        varToCheck.nameToUse = nameToUse;
    }

    private checkAndGetNameToUse(nameToCheck: string): string {
        let r = nameToCheck;
        if (this.isNameUsed(r)) {
            for (let i = 1; ; i++) {
                r = nameToCheck + '_' + i.toString();
                if (!this.isNameUsed(r))
                    break;
            }
        }
        return r;
    }

    private isNameUsed(name: string) {
        return this.allDefinedVariables.findIndex(v => v.nameToUse === name || v.suggestedName === name) >= 0;
    }

}
