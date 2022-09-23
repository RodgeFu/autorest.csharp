// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoRest.CSharp.Generation.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    internal class MgmtExplorerCodeSegment
    {
        internal enum CodeSegmentScope
        {
            Global = 0,
            Local
        }

        public string Key { get; init; }
        public string SuggestName { get; init; }
        public CodeSegmentScope Scope { get; set; } = CodeSegmentScope.Local;

        public StringBuilder Code { get; set; } = new StringBuilder();
        public List<string> Namespaces { get; set; } = new List<string>();
        public List<MgmtExplorerCodeSegmentVariable> Dependencies { get; set; } = new List<MgmtExplorerCodeSegmentVariable>();
        public List<MgmtExplorerCodeSegmentVariable> OutputResult { get; set; } = new List<MgmtExplorerCodeSegmentVariable>();
        public List<MgmtExplorerCodeSegmentVariable> Variables { get; set; } = new List<MgmtExplorerCodeSegmentVariable>();
        public List<MgmtExplorerCodeSegmentParameter> Parameters { get; set; } = new List<MgmtExplorerCodeSegmentParameter>();

        public MgmtExplorerCodeSegment(string key, string suggestedName)
        {
            this.Key = key;
            this.SuggestName = suggestedName;
        }
    }

    internal record class MgmtExplorerCodeSegmentCSharpType(string name, string @namespace, bool isGeneric, MgmtExplorerCodeSegmentCSharpType[] arguments);

    internal record class MgmtExplorerCodeSegmentVariable(string key, string suggestedName, MgmtExplorerCodeSegmentCSharpType type);

    internal record class MgmtExplorerCodeSegmentParameter(string key, string suggestedName, MgmtExplorerCodeSegmentCSharpType type, string? description, string? defaultValue);
}
