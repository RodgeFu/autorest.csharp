// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using AutoRest.SdkExplorer.Model.Code;

namespace AutoRest.SdkExplorer.Model.Schema
{
    public class SchemaStore
    {
        private static SchemaStore? _current;
        /// <summary>
        /// The current SchemaStore to use.
        /// set this static property to make things easier instead of passing schemaStore everywhere
        /// </summary>
        public static SchemaStore Current
        {
            get
            {
                if (_current == null)
                    _current = new SchemaStore();
                return _current;
            }
            set { _current = value; }
        }

        // store schemas per type so that they can be handled easily when passing through json/yaml
        public Dictionary<string, SchemaObject> ObjectSchemas { get; set; } = new Dictionary<string, SchemaObject>();
        public Dictionary<string, SchemaEnum> EnumSchemas { get; set; } = new Dictionary<string, SchemaEnum>();
        public Dictionary<string, SchemaNone> NoneSchemas { get; set; } = new Dictionary<string, SchemaNone>();

        public SchemaStore()
        {
        }

        ///// <summary>
        ///// Reset the existing store to empty status
        ///// </summary>
        ///// <returns>a copy of existing store will be returned</returns>
        //public SchemaStore ResetStore()
        //{
        //    SchemaStore ss = new SchemaStore()
        //    {
        //        ObjectSchemas = this.ObjectSchemas,
        //        EnumSchemas = this.EnumSchemas,
        //        NoneSchemas = this.NoneSchemas
        //    };
        //    this.ObjectSchemas = new Dictionary<string, SchemaObject>();
        //    this.EnumSchemas = new Dictionary<string, SchemaEnum>();
        //    this.NoneSchemas = new Dictionary<string, SchemaNone>();
        //    return ss;
        //}

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
            else if (this.NoneSchemas.ContainsKey(schemaKey))
                return this.NoneSchemas[schemaKey];
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
                    this.NoneSchemas[schema.SchemaKey] = na;
                    break;
                default:
                    throw new InvalidOperationException("Unknown schema: " + schema.GetType().FullName);
            };
            return schema;
        }
    }
}
