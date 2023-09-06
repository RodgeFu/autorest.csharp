// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace AutoRest.SdkExplorer.Model.Azure
{
    public class AzureResourceIdentifierSegment
    {
        public AzureResourceIdentifierSegment(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }
        public string Type { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Type + (string.IsNullOrEmpty(Value) ? "" : $"/{Value}");
        }
    }
}
