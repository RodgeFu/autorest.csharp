// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.Generation.Writers;
using AutoRest.CSharp.MgmtExplorer.Contract;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal class MgmtExplorerCodeSegmentWriter
    {
        private CodeWriter _codeWriter = new CodeWriter();
        private List<MgmtExplorerCodeSegmentParameter> _parameters = new List<MgmtExplorerCodeSegmentParameter>();
        private List<MgmtExplorerCodeSegmentVariable> _variables = new List<MgmtExplorerCodeSegmentVariable>();

        // just for debugging and troubleshooting purpose
        private CodeWriter? _extraCodeWriter = null;

        public MgmtExplorerCodeSegmentWriter(CodeWriter? extraWriter = null)
        {
            // TODO: remove this short workaround
            _extraCodeWriter = null;
        }

        public void UseNamespace(string ns)
        {
            if (ns != null)
            {
                this._codeWriter.UseNamespace(ns);
                this._extraCodeWriter?.UseNamespace(ns);
            }
        }

        public void AddCodeSegmentParameter(MgmtExplorerCodeSegmentParameter param)
        {
            this._parameters.Add(param);
        }

        public void AddCodeSegmentVariable(MgmtExplorerCodeSegmentVariable variable)
        {
            this._variables.Add(variable);
        }

        public void Line(FormattableString? str = null)
        {
            if (str != null)
            {
                this._codeWriter.Line(str);
                this._extraCodeWriter?.Line(str);
            }
            else
            {
                this._codeWriter.Line();
                this._extraCodeWriter?.Line();
            }
        }

        public void LineRaw(string str)
        {
            this._codeWriter.LineRaw(str);
            this._extraCodeWriter?.LineRaw(str);
        }

        public void Append(FormattableString str)
        {
            this._codeWriter.Append(str);
            this._extraCodeWriter?.Append(str);
        }

        public void AppendRaw(string str)
        {
            this._codeWriter.AppendRaw(str);
            this._extraCodeWriter?.AppendRaw(str);
        }

        public void RemoveTrailingComma()
        {
            this._codeWriter.RemoveTrailingComma();
            this._extraCodeWriter?.RemoveTrailingComma();
        }

        public void WriteToCodeSegment(MgmtExplorerCodeSegment codeSegment)
        {
            codeSegment.Code += this._codeWriter.ToString(false);
            codeSegment.UsingNamespaces = codeSegment.UsingNamespaces.Concat(this._codeWriter.Namespaces).Distinct().ToList();
            codeSegment.Parameters.AddRange(this._parameters);
            codeSegment.Variables.AddRange(this._variables);
        }

        public void SetCodeSegmentFunction(MgmtExplorerCodeSegment codeSegment)
        {
            string suggestedName = codeSegment.SuggestName!;
            MgmtExplorerCSharpType returnType;
            MgmtExplorerCodeSegmentFunction sf;
            string returnStatement = "";
            switch (codeSegment.OutputResult.Count)
            {
                case 0:
                    returnType = new MgmtExplorerCSharpType(typeof(void));
                    sf = new MgmtExplorerCodeSegmentFunction(suggestedName, returnType);
                    sf.FunctionInvoke =
                        $"{sf.Key}({string.Join(", ", codeSegment.Dependencies.Select(dep => dep.Key))})";
                    break;
                case 1:
                    returnType = codeSegment.OutputResult[0].Type!;
                    sf = new MgmtExplorerCodeSegmentFunction(suggestedName, returnType);
                    returnStatement = $"return {codeSegment.OutputResult[0].Key};";
                    sf.FunctionInvoke =
                        $"global::{returnType.FullNameWithNamespace} {codeSegment.OutputResult[0].Key} = {sf.Key}({string.Join(", ", codeSegment.Dependencies.Select(dep => dep.Key))})";
                    break;
                default:
                    throw new NotSupportedException("multiple return result is not supported");
            }

            sf.FunctionWrap =
                $"global::{returnType.FullNameWithNamespace} {sf.Key}({string.Join(", ", codeSegment.Dependencies.Select(dep =>  $"global::{dep.Type!.FullNameWithNamespace} {dep.Key}"))})\n" +
                "{\n" +
                $"{MgmtExplorerCodeGenUtility.TAB_STRING}{MgmtExplorerCodeSegmentFunction.FUNC_CODESEGMENT_CODE}\n" +
                $"{MgmtExplorerCodeGenUtility.TAB_STRING}{returnStatement}\n" +
                "}\n";
            codeSegment.Function = sf;
        }
    }
}
