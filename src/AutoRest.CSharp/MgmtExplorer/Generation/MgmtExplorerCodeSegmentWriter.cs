// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Generation.Writers;
using AutoRest.CSharp.MgmtExplorer.Contract;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal class MgmtExplorerCodeSegmentWriter
    {
        private CodeWriter _codeWriter = new CodeWriter();
        private List<MgmtExplorerCodeSegmentParameter> _parameters = new List<MgmtExplorerCodeSegmentParameter>();
        private List<MgmtExplorerCodeSegmentVariable> _variables = new List<MgmtExplorerCodeSegmentVariable>();

        // for debugging and troubleshooting purpose
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
            codeSegment.Code.Append(this._codeWriter.ToString(false));
            codeSegment.Namespaces = codeSegment.Namespaces.Concat(this._codeWriter.Namespaces).Distinct().ToList();
            codeSegment.Parameters.AddRange(this._parameters);
            codeSegment.Variables.AddRange(this._variables);
        }
    }
}
