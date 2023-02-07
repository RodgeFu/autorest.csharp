// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.MgmtExplorer.Generation;
using AutoRest.CSharp.Output.Models.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerCSharpType
    {
        public string? Name { get; set; }
        public string? Namespace { get; set; }
        public bool IsValueType { get; set; }
        public bool IsEnum { get; set; }
        public bool IsNullable { get; set; }
        public bool IsGenericType { get; set; }
        public bool IsFrameworkType { get; set; }
        public bool IsList { get; set; }
        public bool IsDictionary { get; set; }
        public bool IsBinaryData { get; set; }
        public List<MgmtExplorerCSharpType> Arguments { get; set; } = new List<MgmtExplorerCSharpType>();
        public string? FullNameWithNamespace { get; set; }
        public string? FullNameWithoutNamespace { get; set; }

        public string? schemaType { get; set; }
        public string? schemaKey { get; set; }

        public MgmtExplorerCSharpType()
        {
        }

        public static MgmtExplorerCSharpType CreateEnumType(string name, string @namespace, bool isNullable, MgmtExplorerSchemaEnum schema)
        {
            var o = new MgmtExplorerCSharpType()
            {
                Name = name,
                Namespace = @namespace,
                IsValueType = true,
                IsEnum = true,
                IsNullable = isNullable,
                IsGenericType = false,
                IsFrameworkType = false,
                IsList = false,
                IsDictionary = false,
                IsBinaryData = false,
                Arguments = new List<MgmtExplorerCSharpType>(),
            };
            o.FullNameWithNamespace = o.GetFullName(true);
            o.FullNameWithoutNamespace = o.GetFullName(false);
            o.SetSchema(schema);
            return o;
        }

        internal MgmtExplorerCSharpType(CSharpType csharpType)
        {
            if (csharpType.IsFrameworkType && csharpType.FrameworkType == typeof(Nullable<>))
            {
                // use the real type and mark it as nullable
                csharpType = csharpType.Arguments[0];
                this.IsNullable = true;
            }
            else if (!csharpType.IsFrameworkType && csharpType.Implementation is SystemObjectType && ((SystemObjectType)csharpType.Implementation).SystemType.IsClass)
            {
                this.IsNullable = true;
            }
            else
            {
                this.IsNullable = csharpType.IsNullable;
            }

            this.Name = csharpType.Name;
            this.Namespace = csharpType.Namespace;
            this.IsValueType = csharpType.IsValueType;
            this.IsEnum = csharpType.IsEnum;
            this.IsGenericType = csharpType.IsGenericType;
            this.IsFrameworkType = csharpType.IsFrameworkType;
            this.Arguments = new List<MgmtExplorerCSharpType>();
            if (IsGenericType)
            {
                foreach (var t in csharpType.Arguments)
                {
                    this.Arguments.Add(new MgmtExplorerCSharpType(t));
                }
            }
            this.FullNameWithNamespace = GetFullName(true);
            this.FullNameWithoutNamespace = GetFullName(false);

            if (csharpType.IsFrameworkType)
            {
                if (TypeFactory.IsList(csharpType) || csharpType.FrameworkType == typeof(List<>))
                    this.IsList = true;

                if (TypeFactory.IsDictionary(csharpType))
                    this.IsDictionary = true;

                if (csharpType.FrameworkType == typeof(BinaryData))
                    this.IsBinaryData = true;
            }

            var schema = MgmtExplorerCodeGenSchemaStore.Instance.CreateAndAddSchema(this, csharpType);
            this.SetSchema(schema);
        }

        private void SetSchema(MgmtExplorerSchemaBase schema)
        {
            this.schemaKey = schema.SchemaKey;
            this.schemaType = schema.SchemaType;
        }

        private string GetFullName(bool includeNamespace)
        {
            string name = includeNamespace ? $"{this.Namespace ?? "__N/A__"}.{this.Name ?? "__N/A__"}" : this.Name ?? "__N/A__";
            if (IsNullable)
                name += "?";
            if (IsGenericType)
            {
                name += "<" + string.Join(", ", this.Arguments.Select(a => a.GetFullName(includeNamespace))) + ">";
            }
            return name;
        }
    }
}
