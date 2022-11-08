// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using AutoRest.CSharp.MgmtExplorer.Contract;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal class MgmtExplorerCodeGenSchemaStore
    {
        public static MgmtExplorerCodeGenSchemaStore Instance { get; private set; } = new MgmtExplorerCodeGenSchemaStore();
        public static void CreateStore()
        {
            MgmtExplorerCodeGenSchemaStore.Instance = new MgmtExplorerCodeGenSchemaStore();
        }

        public Dictionary<string, MgmtExplorerSchemaObject> ObjectSchemas { get; } = new Dictionary<string, MgmtExplorerSchemaObject>();
        public Dictionary<string, MgmtExplorerSchemaEnum> EnumSchemas { get; } = new Dictionary<string, MgmtExplorerSchemaEnum>();

        public MgmtExplorerCodeGenSchemaStore()
        {
        }

        public void AddSchema(string key, MgmtExplorerSchemaBase schema)
        {
            schema.SchemaKey = key;
            switch (schema)
            {
                case MgmtExplorerSchemaObject obj:
                    this.ObjectSchemas[schema.SchemaKey] = obj;
                    break;
                case MgmtExplorerSchemaEnum en:
                    this.EnumSchemas[schema.SchemaKey] = en;
                    break;
                default:
                    throw new InvalidOperationException("Unknown schema: " + schema.GetType().FullName);
            };
        }
    }
}
