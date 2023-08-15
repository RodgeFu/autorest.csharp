// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace AutoRest.SdkExplorer.Model.Code
{
    public class VariableDesc : PlaceHolderDesc
    {
        public TypeDesc? Type { get; set; }

        private VariableDesc()
            : base()
        {
        }

        [JsonConstructor]
        public VariableDesc(string key, string suggestedName, TypeDesc type)
        {
            Key = key;
            SuggestedName = suggestedName;
            Type = type;
        }
    }
}
