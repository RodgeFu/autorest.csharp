// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Output.Models.Types;

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

        private CSharpType? _csharpType;
        internal MgmtExplorerSchemaObject(CSharpType csharpType)
            : base(generateKey(csharpType), SCHEMA_TYPE)
        {
            this._csharpType = csharpType;
        }

        internal void Initialize()
        {
            if (this._csharpType != null && !this._csharpType.IsFrameworkType)
            {
                if (this._csharpType.Implementation is SchemaObjectType)
                {
                    var imp = (SchemaObjectType)this._csharpType.Implementation;

                    this.IsStruct = imp.IsStruct;
                    this.InheritFrom = imp.Inherits == null ? null : new MgmtExplorerCSharpType(imp.Inherits);
                    this.InheritBy = new List<MgmtExplorerCSharpType>();
                    this.Description = imp.Description ?? "";
                    this.Properties = imp.Properties
                        .Select(p => p.FlattenedProperty ?? p)
                        .Where(p => p.Declaration.Accessibility == "public" &&
                            (!p.IsReadOnly || (p.Declaration.Type.IsFrameworkType && (TypeFactory.IsReadWriteList(p.Declaration.Type) || TypeFactory.IsReadWriteDictionary(p.Declaration.Type)))))
                        .Select(p =>
                        {
                            return GenerateProperty(p);
                        }).ToList();
                    this.InitializationConstructor = imp.InitializationConstructor.Signature.Modifiers == Output.Models.MethodSignatureModifiers.Public ? new MgmtExplorerSchemaConstructor(imp.InitializationConstructor) : null;
                    this.SerializationConstructor = imp.SerializationConstructor.Signature.Modifiers == Output.Models.MethodSignatureModifiers.Public ? new MgmtExplorerSchemaConstructor(imp.SerializationConstructor) : null;
                }
                else if (this._csharpType.Implementation is SystemObjectType)
                {
                    var imp = (SystemObjectType)this._csharpType.Implementation;

                    this.IsStruct = false;
                    this.InheritFrom = imp.Inherits == null ? null : new MgmtExplorerCSharpType(imp.Inherits);
                    this.InheritBy = new List<MgmtExplorerCSharpType>();
                    this.Description = ""; // not supported in SystemObjectType...
                    this.Properties = imp.Properties
                        .Select(p => p.FlattenedProperty ?? p)
                        .Where(p => p.Declaration.Accessibility == "public" &&
                         (!p.IsReadOnly || (p.Declaration.Type.IsFrameworkType && (TypeFactory.IsReadWriteList(p.Declaration.Type) || TypeFactory.IsReadWriteDictionary(p.Declaration.Type)))))
                        .Select(p => GenerateProperty(p)).ToList();
                    this.InitializationConstructor = imp.InitializationConstructor.Signature.Modifiers == Output.Models.MethodSignatureModifiers.Public ? new MgmtExplorerSchemaConstructor(imp.InitializationConstructor) : null;
                    this.SerializationConstructor = imp.SerializationConstructor.Signature.Modifiers == Output.Models.MethodSignatureModifiers.Public ? new MgmtExplorerSchemaConstructor(imp.SerializationConstructor) : null;

                    UpdateSystemObjectType(imp);
                }
            }
        }

        private MgmtExplorerSchemaProperty GenerateProperty(ObjectTypeProperty p)
        {
            if (p is FlattenedObjectTypeProperty fp)
                return new MgmtExplorerSchemaProperty(fp);
            else
                return new MgmtExplorerSchemaProperty(p);
        }

        private bool IsConstructorMatch(ConstructorInfo a, MgmtExplorerSchemaConstructor b)
        {
            var ap = a.GetParameters();
            if (ap.Length != b.MethodParameters.Count)
                return false;
            for (int i = 0; i < ap.Length; i++)
            {
                if (ap[i].ParameterType.Name != b.MethodParameters[i].Type?.Name)
                    return false;
            }
            return true;
        }

        // some system object type's metadata is not set accurate, try to fix them
        private void UpdateSystemObjectType(SystemObjectType sot)
        {
            var systemType = sot.SystemType;

            // some ctor should be Internal instead of public, seems a bug at SystemObjectType.cs line 136, the ctor can be NonPublic (not sure whehter we should fix there, so fix here as a temp workaround)
            var ctors = systemType.GetConstructors();
            if (this.InitializationConstructor != null)
            {
                if (ctors.FirstOrDefault(ctor => this.IsConstructorMatch(ctor, this.InitializationConstructor)) == null)
                    this.InitializationConstructor = null;
            }
            if (this.SerializationConstructor != null)
            {
                if (ctors.FirstOrDefault(ctor => this.IsConstructorMatch(ctor, this.SerializationConstructor)) == null)
                    this.SerializationConstructor = null;
            }

            foreach (var prop in this.Properties)
            {
                if (!string.IsNullOrEmpty(prop.Name))
                {
                    var oriProp = systemType.GetProperty(prop.Name);
                    if (oriProp != null)
                    {
                        // not handled properly in Model either
                        prop.IsReadonly = !oriProp.CanWrite;
                    }
                    else
                    {
                        throw new InvalidOperationException("Can't find public property with name " + prop.Name);
                    }
                }
            }

            //if (this.SchemaKey == typeof(TrackedResourceData).FullName)
            //{
            //    var tags = this.Properties.Find(p => p.Name == "Tags");
            //    if (tags != null)
            //        tags.IsReadonly = true;
            //}
            //else if (this.SchemaKey == typeof(Azure.ResourceManager.Resources.Models.ExtendedLocation).FullName)
            //{
            //    // the ctor should be Internal instead of public, seems a bug at SystemObjectType.cs line 136, the ctor can be NonPublic (not sure whehter we should fix there, so fix here as a temp workaround)
            //    this.SerializationConstructor = null;
            //}
            //else if (this.SchemaKey == typeof(Azure.ResourceManager.Models.ManagedServiceIdentity).FullName)
            //{
            //    // the ctor should be Internal instead of public, seems a bug at SystemObjectType.cs line 136, the ctor can be NonPublic (not sure whehter we should fix there, so fix here as a temp workaround)
            //    this.SerializationConstructor = null;
            //}
        }

        public MgmtExplorerSchemaObject()
            : base()
        {
        }

    }
}
