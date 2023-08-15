// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace AutoRest.SdkExplorer.Model.Schema
{
    public class SchemaEnumValue
    {
        public string? Value { get; set; }
        public string? InternalValue { get; set; }
        public string Description { get; set; } = string.Empty;

        public SchemaEnumValue()
        {

        }
    }
}
