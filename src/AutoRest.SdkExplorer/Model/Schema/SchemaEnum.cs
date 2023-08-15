// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoRest.SdkExplorer.Model.Schema
{
    public class SchemaEnum : SchemaBase
    {
        public const string SCHEMA_TYPE = "ENUM_SCHEMA";

        public List<SchemaEnumValue> Values { get; set; } = new List<SchemaEnumValue>();
        public string Description { get; set; } = string.Empty;

        private SchemaEnum()
            : base("OnlyForYamlDeserializor", SCHEMA_TYPE)
        {
        }

        [JsonConstructor]
        public SchemaEnum(string schemaKey)
            : base(schemaKey, SCHEMA_TYPE)
        {
        }

    }
}
