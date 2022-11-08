// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerCodeSegmentParameter
    {
        public string? Key { get; set; }
        public string? SuggestedName { get; set; }
        public string? ModelName { get; set; }
        public string? SerializerName { get; set; }
        public MgmtExplorerCSharpType? Type { get; set; }
        public string? Description { get; set; }
        public string? DefaultValue { get; set; }

        public MgmtExplorerCodeSegmentParameter()
        {
        }

        public MgmtExplorerCodeSegmentParameter(string key, string suggestedName, string modelName, string serializerName, MgmtExplorerCSharpType type, string? description, string? defaultValue)
        {
            this.Key = key;
            this.SuggestedName = suggestedName;
            this.Type = type;
            this.Description = description;
            this.DefaultValue = defaultValue;
            this.ModelName = modelName;
            this.SerializerName = serializerName;
        }
    }
}
