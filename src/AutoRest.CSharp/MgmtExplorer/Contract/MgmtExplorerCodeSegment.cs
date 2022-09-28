// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerCodeSegment
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }
        [JsonPropertyName("suggestName")]
        public string? SuggestName { get; set; }
        [JsonPropertyName("code")]
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string? Code { get; set; }
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

        public MgmtExplorerCodeSegment()
        {
        }

        public MgmtExplorerCodeSegment(string key, string suggestedName)
        {
            this.Key = key;
            this.SuggestName = suggestedName;
        }
    }
}
