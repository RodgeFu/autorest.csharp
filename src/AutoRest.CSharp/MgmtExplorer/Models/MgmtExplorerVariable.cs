// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Generation.Writers;
using AutoRest.CSharp.MgmtExplorer.Extensions;
using AutoRest.SdkExplorer.Model.Code;

namespace AutoRest.CSharp.MgmtExplorer.Models
{
    internal class MgmtExplorerVariable : MgmtExplorerPlaceHolder
    {
        public string SuggestedName => this.DefaultReplacement!;
        public CodeWriterDeclaration KeyDeclaration { get; init; }
        private VariableDesc _codeSegmentVariable;

        public MgmtExplorerVariable(string suggestedName, CSharpType type)
            :base(
                 $"__VS__{suggestedName}__VE__",
                 type,
                 suggestedName)
        {
            this.KeyDeclaration = new CodeWriterDeclaration(this.Key);
            this._codeSegmentVariable = new VariableDesc(
                this.Key, this.SuggestedName, this.Type.CreateSeTypeDesc());
        }

        public VariableDesc AsCodeSegmentVariable()
        {
            return _codeSegmentVariable;
        }

    }
}
