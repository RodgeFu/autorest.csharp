// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Output.Models.Types;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerSchemaObject : MgmtExplorerSchemaBase
    {
        public const string SCHEMA_TYPE = "OBJECT_SCHEMA";

        public List<MgmtExplorerSchemaProperty> Properties { get; set; } = new List<MgmtExplorerSchemaProperty>();
        public MgmtExplorerCSharpType? InheritFrom { get; set; }
        // TODO: discriminator support
        public List<MgmtExplorerCSharpType> InheritBy { get; set; } = new List<MgmtExplorerCSharpType>();
        public MgmtExplorerSchemaConstructor? InitializationConstructor { get; set; }
        public MgmtExplorerSchemaConstructor? SerializationConstructor { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsStruct { get; set; }
        /// <summary>
        /// sometimes, enum will be made as class/struct to void compatibility issue of real enum type
        /// set this to true in this case
        /// </summary>
        public bool IsEnum { get; set; } = false;

        /// <summary>
        /// Provide Enum Value when IsEnum is true
        /// </summary>
        public List<MgmtExplorerSchemaEnumValue> EnumValues { get; set; } = new List<MgmtExplorerSchemaEnumValue>();

        internal MgmtExplorerSchemaObject(CSharpType csharpType)
            : base(generateKey(csharpType), SCHEMA_TYPE)
        {
            if (!csharpType.IsFrameworkType)
            {
                if (csharpType.Implementation is SchemaObjectType)
                {
                    var imp = (SchemaObjectType)csharpType.Implementation;

                    this.IsStruct = imp.IsStruct;
                    this.InheritFrom = imp.Inherits == null ? null : new MgmtExplorerCSharpType(imp.Inherits);
                    this.InheritBy = new List<MgmtExplorerCSharpType>();
                    this.Description = imp.Description ?? "";
                    this.Properties = imp.Properties.Where(p => p.Declaration.Accessibility == "public" && (!p.IsReadOnly || TypeFactory.IsCollectionType(p.Declaration.Type))).Select(p => new MgmtExplorerSchemaProperty(p)).ToList();
                    this.InitializationConstructor = imp.InitializationConstructor.Signature.Modifiers == Output.Models.MethodSignatureModifiers.Public ? new MgmtExplorerSchemaConstructor(imp.InitializationConstructor) : null;
                    this.SerializationConstructor = imp.SerializationConstructor.Signature.Modifiers == Output.Models.MethodSignatureModifiers.Public ? new MgmtExplorerSchemaConstructor(imp.SerializationConstructor) : null;
                }
                else if (csharpType.Implementation is SystemObjectType)
                {
                    var imp = (SystemObjectType)csharpType.Implementation;

                    this.IsStruct = false;
                    this.InheritFrom = imp.Inherits == null ? null : new MgmtExplorerCSharpType(imp.Inherits);
                    this.InheritBy = new List<MgmtExplorerCSharpType>();
                    this.Description = ""; // not supported in SystemObjectType...
                    this.Properties = imp.Properties.Where(p => p.Declaration.Accessibility == "public" && (!p.IsReadOnly || TypeFactory.IsCollectionType(p.Declaration.Type))).Select(p => new MgmtExplorerSchemaProperty(p)).ToList();
                    this.InitializationConstructor = new MgmtExplorerSchemaConstructor(imp.InitializationConstructor);
                    this.SerializationConstructor = new MgmtExplorerSchemaConstructor(imp.SerializationConstructor);
                }
            }
        }

        public MgmtExplorerSchemaObject()
            : base()
        {

        }

    }
}
