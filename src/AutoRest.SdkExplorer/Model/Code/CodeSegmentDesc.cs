// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using AutoRest.SdkExplorer.Model.Example;
using AutoRest.SdkExplorer.Model.Schema;
using AutoRest.SdkExplorer.Model.Hint;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AutoRest.SdkExplorer.Model.Code
{
    public class CodeSegmentDesc : PlaceHolderDesc
    {
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string? Code { get; set; }
        public List<string> UsingNamespaces { get; set; } = new List<string>();
        public List<VariableDesc> Dependencies { get; set; } = new List<VariableDesc>();
        public List<VariableDesc> OutputResult { get; set; } = new List<VariableDesc>();
        public List<VariableDesc> Variables { get; set; } = new List<VariableDesc>();
        public List<ParameterDesc> Parameters { get; set; } = new List<ParameterDesc>();
        public FunctionDesc? Function { get; set; }
        public bool IsShareable { get; set; }

        public CodeSegmentDesc()
            : base()
        {
        }

        public CodeSegmentDesc(string key, string suggestedName)
            : base(key, suggestedName)
        {
        }

        public string? GetCodeWithSuggestedName(bool useDepencendySuggestedName = true, bool useVariableSuggestedName = true, bool useParameterSuggestedName = false)
        {
            if (this.Code == null)
                return null;
            else
            {
                string code = this.Code;

                if (useDepencendySuggestedName)
                    this.Dependencies.ForEach(dep => code = dep.Apply(code));
                if (useVariableSuggestedName)
                    this.Variables.ForEach(v => code = v.Apply(code));
                if (useParameterSuggestedName)
                    this.Parameters.ForEach(p => code = p.Apply(code));

                return code;
            }
        }

        public IEnumerable<FieldHint> GetFieldHints(string apiName, ExampleDesc ex, SchemaStore? schemaStore = null)
        {
            foreach (var param in this.Parameters)
                foreach (var r in param.GetFieldHints(apiName, ex, schemaStore))
                    yield return r;
        }
    }
}
