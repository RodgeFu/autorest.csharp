// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using AutoRest.CSharp.Generation.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    internal class MgmtExplorerCodeSegment
    {
        [JsonPropertyName("key")]
        public string Key { get; init; }
        [JsonPropertyName("suggestName")]
        public string SuggestName { get; init; }
        [JsonPropertyName("code")]
        public string Code { get; set; } = String.Empty;
        [JsonPropertyName("usingNamespaces")]
        public List<string> UsingNamespaces { get; set; } = new List<string>();
        [JsonPropertyName("dependencies")]
        public List<MgmtExplorerCodeSegmentVariable> Dependencies { get; set; } = new List<MgmtExplorerCodeSegmentVariable>();
        [JsonPropertyName("outputResult")]
        public List<MgmtExplorerCodeSegmentVariable> OutputResult { get; set; } = new List<MgmtExplorerCodeSegmentVariable>();
        [JsonPropertyName("variables")]
        public List<MgmtExplorerCodeSegmentVariable> Variables { get; set; } = new List<MgmtExplorerCodeSegmentVariable>();
        [JsonPropertyName("parameters")]
        public List<MgmtExplorerCodeSegmentParameter> Parameters { get; set; } = new List<MgmtExplorerCodeSegmentParameter>();

        public MgmtExplorerCodeSegment(string key, string suggestedName)
        {
            this.Key = key;
            this.SuggestName = suggestedName;
        }
    }

    internal record class MgmtExplorerCodeSegmentVariable(string key, string suggestedName, MgmtExplorerCodeSegmentCSharpType type);

    internal record class MgmtExplorerCodeSegmentParameter(string key, string suggestedName, MgmtExplorerCodeSegmentCSharpType type, string? description, string? defaultValue);
}
