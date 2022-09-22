// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    internal enum CodeSegmentScope
    {
        Global = 0,
        Local
    }

    internal class CodeSegment
    {
        public class KnownCodeSegmentKey
        {
            public const string ARM_CLIENT = "ARM_CLIENT";
            public const string TENANT_RESOURCE = "TENANT_RESOURCE";
            public const string SUBSCRIPTION_RESOURCE = "SUBSCRIPTION_RESOURCE";
            public const string RESOURCEGROUP_RESOURCE = "RESOURCEGROUP_RESOURCE_<##resourceGroupName##>";
        }

        public string Key { get; set; }
        public string Code { get; set; } = string.Empty;
        public CodeSegmentScope Scope { get; set; }
        public string? SuggestName { get; set; }

        public List<string> Namespaces { get; set; } = new List<string>();
        public List<CodeSegmentVariable> Dependencies { get; set; } = new List<CodeSegmentVariable>();
        public List<CodeSegmentVariable> OutputResult { get; set; } = new List<CodeSegmentVariable>();
        public List<CodeSegmentParameterVariable> Parameters { get; set; } = new List<CodeSegmentParameterVariable>();

        public CodeSegment(string key, string suggestedName, CodeSegmentScope scope)
        {
            this.Key = key;
            this.Scope = scope;
            this.SuggestName = suggestedName;
        }


    }
}
