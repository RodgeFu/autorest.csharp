// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.SdkExplorer.Model.Code;

namespace AutoRest.SdkExplorer.Model.Schema
{
    public class SchemaMethodParameter

    {
        public string? Name { get; set; }
        public string? RelatedPropertySerializerPath { get; set; }
        public TypeDesc? Type { get; set; }
        public bool IsOptional { get; set; }
        public string? DefaultValue { get; set; }
        public string Description { get; set; } = string.Empty;

        public SchemaMethodParameter()
        {

        }
    }
}
