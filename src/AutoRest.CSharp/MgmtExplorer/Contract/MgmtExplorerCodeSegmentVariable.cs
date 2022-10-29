// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerCodeSegmentVariable
    {
        public string? Key { get; set; }
        public string? SuggestedName { get; set; }
        public MgmtExplorerCSharpType? Type { get; set; }

        public MgmtExplorerCodeSegmentVariable(string key, string suggestedName, MgmtExplorerCSharpType type)
        {
            Key = key;
            SuggestedName = suggestedName;
            Type = type;
        }

        public MgmtExplorerCodeSegmentVariable()
        {

        }
    }
}
