// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Input;
using AutoRest.CSharp.Mgmt.Output;
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

        public MgmtExplorerCSharpType()
        {
        }

        internal MgmtExplorerCSharpType(CSharpType csharpType)
        {
            this.Name = csharpType.Name;
            this.Namespace = csharpType.Namespace;
            this.IsValueType = csharpType.IsValueType;
            this.IsEnum = csharpType.IsEnum;
            this.IsNullable = csharpType.IsNullable;
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
                if (TypeFactory.IsList(csharpType))
                    this.IsList = true;

                if (TypeFactory.IsDictionary(csharpType))
                    this.IsDictionary = true;

                if (csharpType.FrameworkType == typeof(BinaryData))
                    this.IsBinaryData = true;

                // TODO: check all used complex framework type, handle raw type, there shouldn't be many complex type, let's see whether we can handle them hard-code to keep the code simple
            }
            else if (csharpType.Implementation is EnumType)
            {
                var imp = (EnumType)csharpType.Implementation;

                var schema = new MgmtExplorerSchemaEnum();
                schema.Values = imp.Values.Select(v => new MgmtExplorerSchemaEnumValue(v)).ToList();
                schema.SchemaKey = this.FullNameWithNamespace;
                MgmtExplorerCodeGenSchemaStore.Instance.AddSchema(this.FullNameWithNamespace, schema);
            }
            else if (csharpType.Implementation is MgmtObjectType)
            {
                var imp = (MgmtObjectType)csharpType.Implementation;

                var schema = new MgmtExplorerSchemaObject();
                schema.IsStruct = imp.IsStruct;
                schema.InheritFrom = imp.Inherits == null ? null : new MgmtExplorerCSharpType(imp.Inherits);
                schema.InheritBy = new List<MgmtExplorerCSharpType>();
                schema.Description = imp.Description ?? "";
                schema.Properties = imp.Properties.Where(p => p.Declaration.Accessibility == "public" && !p.IsReadOnly).Select(p => new MgmtExplorerSchemaProperty(p)).ToList();
                schema.InitializationConstructor = new MgmtExplorerSchemaConstructor(imp.InitializationConstructor);
                schema.SchemaKey = this.FullNameWithNamespace;
                MgmtExplorerCodeGenSchemaStore.Instance.AddSchema(this.FullNameWithNamespace, schema);
            }
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
