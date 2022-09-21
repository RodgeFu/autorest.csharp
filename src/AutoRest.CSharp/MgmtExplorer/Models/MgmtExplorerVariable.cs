// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Generation.Writers;

namespace AutoRest.CSharp.MgmtExplorer.Models
{
    internal class MgmtExplorerVariable
    {
        public CodeWriterDeclaration Declaration { get; set; }
        public CSharpType Type { get; set; }

        public MgmtExplorerVariable(string varName, CSharpType type)
            : this(new CodeWriterDeclaration(varName), type)
        {

        }

        public MgmtExplorerVariable(CodeWriterDeclaration variable, CSharpType type)
        {
            this.Declaration = variable;
            this.Type = type;
        }

        public override string ToString()
        {
            return this.Declaration.ActualName;
        }
    }
}
