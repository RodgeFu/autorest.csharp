// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using AutoRest.CSharp.MgmtExplorer.Models;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{

    internal class CodeSegmentVariable
    {
        internal record class CodeSegmentVariableKey(string CodeSegmentKey, string VariableKey);

        public CodeSegmentVariableKey Key;
        public string SuggestName { get; set; }
        public string TypeName { get; set; }
        public string TypeNamespace { get; set; }

        public CodeSegmentVariable(CodeSegmentVariableKey key, MgmtExplorerVariable var)
        {
            Key = key;
            SuggestName = var.Declaration.ActualName;
            TypeName = var.Type.Name;
            TypeNamespace = var.Type.Namespace;
        }
    }
}
