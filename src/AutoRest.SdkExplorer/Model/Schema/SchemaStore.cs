// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using AutoRest.SdkExplorer.Model.Code;

namespace AutoRest.SdkExplorer.Model.Schema
{
    public class SchemaStore
    {
        public static SchemaStore Instance { get; } = new SchemaStore();

        // store schemas per type so that they can be handled easily when passing through json/yaml
        public Dictionary<string, SchemaObject> ObjectSchemas { get; private set; } = new Dictionary<string, SchemaObject>();
        public Dictionary<string, SchemaEnum> EnumSchemas { get; private set; } = new Dictionary<string, SchemaEnum>();
        public Dictionary<string, SchemaNone> NoneSchema { get; private set; } = new Dictionary<string, SchemaNone>();

        public SchemaStore()
        {
        }

        public void Reset()
        {
            this.ObjectSchemas = new Dictionary<string, SchemaObject>();
            this.EnumSchemas = new Dictionary<string, SchemaEnum>();
            this.NoneSchema = new Dictionary<string, SchemaNone>();
        }

        public SchemaBase? GetSchemaFromStore(TypeDesc type)
        {
            if (type.SchemaKey == null)
                throw new ArgumentException("type.schemaKey is null");
            string key = type.SchemaKey;
            return GetSchemaFromStore(key);
        }

        public SchemaBase? GetSchemaFromStore(string schemaKey)
        {
            if (this.ObjectSchemas.ContainsKey(schemaKey))
                return this.ObjectSchemas[schemaKey];
            else if (this.EnumSchemas.ContainsKey(schemaKey))
                return this.EnumSchemas[schemaKey];
            else if (this.NoneSchema.ContainsKey(schemaKey))
                return this.NoneSchema[schemaKey];
            else
                return null;
        }

        public SchemaBase AddSchema(SchemaBase schema)
        {
            switch (schema)
            {
                case SchemaObject obj:
                    this.ObjectSchemas[schema.SchemaKey] = obj;
                    break;
                case SchemaEnum en:
                    this.EnumSchemas[schema.SchemaKey] = en;
                    break;
                case SchemaNone na:
                    this.NoneSchema[schema.SchemaKey] = na;
                    break;
                default:
                    throw new InvalidOperationException("Unknown schema: " + schema.GetType().FullName);
            };
            return schema;
        }
    }
}
