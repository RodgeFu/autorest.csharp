// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Mgmt.Output;
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
        public bool isGenericType { get; set; }
        public List<MgmtExplorerCSharpType> Arguments { get; set; } = new List<MgmtExplorerCSharpType>();
        public string? FullNameWithNamespace { get; set; }
        public string? FullNameWithoutNamespace { get; set; }

        public MgmtExplorerSchemaObject? ObjectSchema { get; set; }
        public MgmtExplorerSchemaEnum? EnumSchema { get; set; }

        public MgmtExplorerCSharpType()
        {
        }

        internal MgmtExplorerCSharpType(CSharpType csharpType, bool includeSchema = false)
        {
            this.Name = csharpType.Name;
            this.Namespace = csharpType.Namespace;
            this.IsValueType = csharpType.IsValueType;
            this.IsEnum = csharpType.IsEnum;
            this.IsNullable = csharpType.IsNullable;
            this.isGenericType = csharpType.IsGenericType;
            this.Arguments = new List<MgmtExplorerCSharpType>();
            if (isGenericType)
            {
                foreach (var t in csharpType.Arguments)
                {
                    this.Arguments.Add(new MgmtExplorerCSharpType(t));
                }
            }
            this.FullNameWithNamespace = GetFullName(true);
            this.FullNameWithoutNamespace = GetFullName(false);

            if (includeSchema)
            {
                if (csharpType.IsFrameworkType)
                {
                    // TODO: check all used framework type, handle raw type, there shouldn't be many complex type, let's see whether we can handle them hard-code to keep the code simple
                }
                else if (csharpType.Implementation is EnumType)
                {
                    var imp = (EnumType)csharpType.Implementation;

                    this.EnumSchema = new MgmtExplorerSchemaEnum();
                    this.EnumSchema.Values = imp.Values.Select(v => new MgmtExplorerSchemaEnumValue(v)).ToList();
                }
                else if (csharpType.Implementation is MgmtObjectType)
                {
                    var imp = (MgmtObjectType)csharpType.Implementation;

                    this.ObjectSchema = new MgmtExplorerSchemaObject();
                    this.ObjectSchema.IsStruct = imp.IsStruct;
                    this.ObjectSchema.InheritFrom = imp.Inherits == null ? null : new MgmtExplorerCSharpType(imp.Inherits, true /* includeObjectSchema */);
                    // TODO: add support for discriminator
                    this.ObjectSchema.InheritBy = new List<MgmtExplorerCSharpType>();
                    this.ObjectSchema.Description = imp.Description ?? "";
                    this.ObjectSchema.Properties = imp.Properties.Where(p => p.Declaration.Accessibility == "public" && !p.IsReadOnly).Select(p => new MgmtExplorerSchemaProperty(p)).ToList();
                    this.ObjectSchema.InitializationConstructor = new MgmtExplorerSchemaConstructor(imp.InitializationConstructor);
                }
            }
        }

        private string GetFullName(bool includeNamespace)
        {
            string name = includeNamespace ? $"{this.Namespace ?? "__N/A__"}.{this.Name ?? "__N/A__"}" : this.Name ?? "__N/A__";
            if (IsNullable)
                name += "?";
            if (isGenericType)
            {
                name += "<" + string.Join(", ", this.Arguments.Select(a => a.GetFullName(includeNamespace))) + ">";
            }
            return name;
        }
    }
}
