// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace AutoRest.SdkExplorer.Model.Schema
{
    public class SchemaNone : SchemaBase
    {
        public const string SCHEMA_TYPE = "NONE_SCHEMA";

        public string Reason { get; set; } = string.Empty;

        private SchemaNone()
            : base("OnlyForYamlDeserializor", SCHEMA_TYPE)
        {
        }

        [JsonConstructor]
        public SchemaNone(string schemaKey)
            : base(schemaKey, SCHEMA_TYPE)
        {
        }
    }
}
