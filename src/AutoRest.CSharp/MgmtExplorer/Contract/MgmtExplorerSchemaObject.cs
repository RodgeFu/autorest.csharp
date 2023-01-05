// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Output.Models;
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
                    this.Properties = imp.Properties
                        .Where(p => p.Declaration.Accessibility == "public" && (!p.IsReadOnly || TypeFactory.IsCollectionType(p.Declaration.Type)))
                        .Select(p =>
                        {
                            return GenerateProperty(p);
                        }).ToList();
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
                    this.Properties = imp.Properties
                        .Where(p => p.Declaration.Accessibility == "public" && (!p.IsReadOnly || TypeFactory.IsCollectionType(p.Declaration.Type)))
                        .Select(p => GenerateProperty(p)).ToList();
                    this.InitializationConstructor = imp.InitializationConstructor.Signature.Modifiers == Output.Models.MethodSignatureModifiers.Public ? new MgmtExplorerSchemaConstructor(imp.InitializationConstructor) : null;
                    this.SerializationConstructor = imp.SerializationConstructor.Signature.Modifiers == Output.Models.MethodSignatureModifiers.Public ? new MgmtExplorerSchemaConstructor(imp.SerializationConstructor) : null;

                    UpdateSystemObjectType(imp);
                }
            }
        }

        private MgmtExplorerSchemaProperty GenerateProperty(ObjectTypeProperty p)
        {
            Stack<ObjectTypeProperty> singleHierarchy = new Stack<ObjectTypeProperty>();
            singleHierarchy.Push(p);
            BuildSingleHierarchy(p, singleHierarchy);
            // for some reason, the single property is handled in codegen directily without model when generating sdk code
            // so have to add special handle here based on model.
            // related code in codegen can be found at ModelWriter.cs line 86
            if (singleHierarchy.Count > 1)
            {
                var propList = singleHierarchy.ToList();
                propList.Reverse();
                var innerProperty = singleHierarchy.Pop();
                var immediateParentProperty = singleHierarchy.Pop();

                string myPropertyName = innerProperty.GetCombinedPropertyName(immediateParentProperty);
                string childPropertyName = p.Equals(immediateParentProperty) ? innerProperty.Declaration.Name : myPropertyName;
                bool isOverridenValueType = innerProperty.Declaration.Type.IsValueType && !innerProperty.Declaration.Type.IsNullable;

                var r = new MgmtExplorerSchemaProperty(innerProperty)
                {
                    Name = myPropertyName,
                    IsRequired = propList.All(s => s.SchemaProperty?.IsRequired == true),
                    IsReadonly = IsSingleHierarchyPropertyReadonly(p, innerProperty),
                    SerializerPath = string.Join("/", propList.Select(s => s.SchemaProperty?.SerializedName ?? s.Declaration.Name)),
                };
                if (isOverridenValueType)
                    r.Type!.IsNullable = true;
                return r;
            }
            else
            {
                return new MgmtExplorerSchemaProperty(p);
            }
        }

        // set readonly per logic from codegen at CodeModel.cs line 100...
        private bool IsSingleHierarchyPropertyReadonly(ObjectTypeProperty property, ObjectTypeProperty innerProperty)
        {
            if (!property.IsReadOnly && innerProperty.IsReadOnly)
            {
                if (HasDefaultPublicCtor(property))
                {
                    return true;
                }
                return false;
            }
            else if (!property.IsReadOnly && !innerProperty.IsReadOnly)
            {
                return false;
            }
            return true;
        }

        // following methods copied fro CodeModel.cs
        internal static bool HasCtorWithSingleParam(ObjectTypeProperty property, ObjectTypeProperty innerProperty)
        {
            var type = property.Declaration.Type;
            if (type.IsFrameworkType)
                return false;

            if (type.Implementation is not ObjectType objType)
                return false;

            foreach (var ctor in objType.Constructors)
            {
                if (ctor.Signature.Modifiers.HasFlag(MethodSignatureModifiers.Public) &&
                    ctor.Signature.Parameters.Count == 1)
                {
                    var paramType = ctor.Signature.Parameters[0].Type;
                    var propertyType = innerProperty.Declaration.Type;
                    if (paramType.Arguments.Length == 0 && paramType.Equals(propertyType))
                        return true;

                    if (paramType.Arguments.Length == 1 && propertyType.Arguments.Length == 1 && paramType.Arguments[0].Equals(propertyType.Arguments[0]))
                        return true;
                }
            }

            return false;
        }

        private static bool HasDefaultPublicCtor(ObjectTypeProperty objectTypeProperty)
        {
            var type = objectTypeProperty.Declaration.Type;
            if (type.IsFrameworkType)
                return true;

            if (type.Implementation is not ObjectType objType)
                return true;

            foreach (var ctor in objType.Constructors)
            {
                if (ctor.Signature.Modifiers.HasFlag(MethodSignatureModifiers.Public) && !ctor.Signature.Parameters.Any())
                    return true;
            }

            return false;
        }

        private void BuildSingleHierarchy(ObjectTypeProperty property, Stack<ObjectTypeProperty> heirarchyStack)
        {
            if (property.IsSinglePropertyObject(out var childProp))
            {
                heirarchyStack.Push(childProp);
                BuildSingleHierarchy(childProp, heirarchyStack);
            }
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
