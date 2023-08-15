// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace AutoRest.SdkExplorer.Model.Schema
{
    public class SchemaBase
    {
        [JsonInclude]
        public string SchemaKey { get; protected set; } = string.Empty;
        [JsonInclude]
        public string SchemaType { get; protected set; } = string.Empty;

        // define a private parameterless constructor to satisfy yaml to deseralize
        private SchemaBase() { }

        [JsonConstructor]
        protected SchemaBase(string schemaKey, string schemaType)
        {
            this.SchemaKey = schemaKey;
            this.SchemaType = schemaType;
        }
    }
}
