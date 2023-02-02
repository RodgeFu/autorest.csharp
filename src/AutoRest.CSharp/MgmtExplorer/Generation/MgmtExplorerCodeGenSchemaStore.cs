// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.Output.Models.Types;
using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal class MgmtExplorerCodeGenSchemaStore
    {
        public static MgmtExplorerCodeGenSchemaStore Instance { get; private set; } = new MgmtExplorerCodeGenSchemaStore();
        public static void CreateStore()
        {
            MgmtExplorerCodeGenSchemaStore.Instance = new MgmtExplorerCodeGenSchemaStore();
        }

        // store schemas per type so that they can be handled easily when passing through json/yaml
        public Dictionary<string, MgmtExplorerSchemaObject> ObjectSchemas { get; } = new Dictionary<string, MgmtExplorerSchemaObject>();
        public Dictionary<string, MgmtExplorerSchemaEnum> EnumSchemas { get; } = new Dictionary<string, MgmtExplorerSchemaEnum>();
        public Dictionary<string, MgmtExplorerSchemaNone> NoneSchema { get; } = new Dictionary<string, MgmtExplorerSchemaNone>();

        public MgmtExplorerCodeGenSchemaStore()
        {
        }

        public MgmtExplorerSchemaBase? GetSchemaFromStore(CSharpType type)
        {
            string key = MgmtExplorerSchemaBase.generateKey(type);
            if (this.ObjectSchemas.ContainsKey(key))
                return this.ObjectSchemas[key];
            else if (this.EnumSchemas.ContainsKey(key))
                return this.EnumSchemas[key];
            else if (this.NoneSchema.ContainsKey(key))
                return this.NoneSchema[key];
            else
                return null;
        }

        public MgmtExplorerSchemaBase CreateAndAddSchema(MgmtExplorerCSharpType type, CSharpType csharpType)
        {
            var schema = this.GetSchemaFromStore(csharpType);
            if (schema != null)
                return schema;

            if (csharpType.IsFrameworkType)
            {
                if (csharpType.FrameworkType == typeof(Nullable<>))
                {
                    throw new InvalidOperationException("Unexpaected Nullable<> which should have been replaced with nullable attribute");
                }
                else if (TypeFactory.IsList(csharpType) ||
                    TypeFactory.IsDictionary(csharpType) ||
                    csharpType.FrameworkType == typeof(List<>) ||
                    csharpType.FrameworkType == typeof(BinaryData) ||
                    csharpType.FrameworkType == typeof(string) ||
                    csharpType.FrameworkType == typeof(int) ||
                    csharpType.FrameworkType == typeof(bool) ||
                    csharpType.FrameworkType == typeof(DateTimeOffset) ||
                    csharpType.FrameworkType == typeof(float) ||
                    csharpType.FrameworkType == typeof(double) ||
                    csharpType.FrameworkType == typeof(System.Int64) ||
                    csharpType.FrameworkType == typeof(Byte[]) ||
                    csharpType.FrameworkType == typeof(Byte))
                {
                    schema = new MgmtExplorerSchemaNone(csharpType, "No schema needed for Raw Framework Data");
                }
                else if (
                    csharpType.FrameworkType == typeof(CancellationToken) ||
                    csharpType.FrameworkType == typeof(ArmClient) ||
                    csharpType.FrameworkType == typeof(WaitUntil) ||
                    csharpType.FrameworkType == typeof(ArmOperation) ||
                    csharpType.FrameworkType == typeof(ArmOperation<>) ||
                    csharpType.FrameworkType == typeof(Response) ||
                    csharpType.FrameworkType == typeof(SystemData) ||
                    csharpType.FrameworkType == typeof(AsyncPageable<>) ||
                    csharpType.FrameworkType == typeof(Pageable<>) ||
                    csharpType.FrameworkType == typeof(TenantResource) ||
                    csharpType.FrameworkType == typeof(SubscriptionResource) ||
                    csharpType.FrameworkType == typeof(ResourceGroupResource) ||
                    csharpType.FrameworkType == typeof(System.IO.Stream))
                {
                    schema = new MgmtExplorerSchemaNone(csharpType, "Not editable type, so no schema needed for now");
                }
                else
                {
                    schema = MgmtExplorerFrameworkTypeSchemaFactory.GetSchema(csharpType);
                }
            }
            else if (csharpType.Implementation is EnumType)
            {
                schema = new MgmtExplorerSchemaEnum(csharpType);
            }
            else if (csharpType.Implementation is SchemaObjectType || csharpType.Implementation is SystemObjectType)
            {
                var objSchema = new MgmtExplorerSchemaObject(csharpType);
                // add first to avoid creating this again when creating schema for children
                this.AddSchema(objSchema);
                objSchema.Initialize();
                schema = objSchema;
            }
            else if (csharpType.Implementation is MgmtTypeProvider)
            {
                schema = new MgmtExplorerSchemaNone(csharpType, "No schema needed for MgmtTypeProvider for now which is the object to trigger operation on");
            }

            if (schema == null)
                throw new InvalidOperationException("Can't get schema for type " + type.FullNameWithNamespace);

            this.AddSchema(schema);
            return schema;
        }

        public MgmtExplorerSchemaBase AddSchema(MgmtExplorerSchemaBase schema)
        {
            switch (schema)
            {
                case MgmtExplorerSchemaObject obj:
                    this.ObjectSchemas[schema.SchemaKey] = obj;
                    break;
                case MgmtExplorerSchemaEnum en:
                    this.EnumSchemas[schema.SchemaKey] = en;
                    break;
                case MgmtExplorerSchemaNone na:
                    this.NoneSchema[schema.SchemaKey] = na;
                    break;
                default:
                    throw new InvalidOperationException("Unknown schema: " + schema.GetType().FullName);
            };
            return schema;
        }
    }
}
