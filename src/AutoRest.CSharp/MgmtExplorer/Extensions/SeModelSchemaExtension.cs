// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using AutoRest.CSharp.Common.Output.Models.Types;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Autorest;
using AutoRest.CSharp.Output.Models.Shared;
using AutoRest.CSharp.Output.Models.Types;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.ManagementGroups;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using AutoRest.SdkExplorer.Model.Code;
using AutoRest.SdkExplorer.Model.Schema;

namespace AutoRest.CSharp.MgmtExplorer.Extensions
{
    internal static class SeModelSchemaExtension
    {
        internal static SchemaBase? GetSchemaFromStore(this CSharpType type)
        {
            return SchemaStore.Current.GetSchemaFromStore(type.GenerateSeSchemaKey());
        }

        internal static SchemaBase GetOrCreateSeSchema(this CSharpType csharpType)
        {
            var schema = csharpType.GetSchemaFromStore();
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
                    schema = csharpType.GetOrCreateSeSchemaNone("No schema needed for Raw Framework Data");
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
                    csharpType.FrameworkType == typeof(ManagementGroupResource) ||
                    csharpType.FrameworkType == typeof(System.IO.Stream))
                {
                    schema = csharpType.GetOrCreateSeSchemaNone("Not editable type, so no schema needed for now");
                }
                else
                {
                    schema = csharpType.GetOrCreateSchemaObjectForFrameworkType();
                }
            }
            else if (csharpType.Implementation is EnumType)
            {
                schema = csharpType.GetOrCreateSeSchemaEnum();
            }
            else if (csharpType.Implementation is SchemaObjectType || csharpType.Implementation is SystemObjectType || csharpType.Implementation is ModelTypeProvider)
            {
                var objSchema = csharpType.GetOrCreateSeSchemaObjectForNonFrameworkType();
                schema = objSchema;
            }
            else if (csharpType.Implementation is MgmtTypeProvider)
            {
                schema = csharpType.GetOrCreateSeSchemaNone("No schema needed for MgmtTypeProvider for now which is the object to trigger operation on");
            }

            if (schema == null)
                throw new InvalidOperationException("Can't get schema for type " + csharpType.GetFullName(true, true));

            return schema;
        }

        /// <summary>
        /// Create and add to store at the same time to avoid create the same type again when creating sub types recursively
        /// </summary>
        /// <param name="csharpType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static SchemaObject GetOrCreateSeSchemaObjectForNonFrameworkType(this CSharpType csharpType)
        {
            if (csharpType.IsFrameworkType)
                throw new InvalidOperationException("type is frameworktype: " + csharpType.Name);

            var schema = csharpType.GetSchemaFromStore();
            if (schema != null)
            {
                if (schema is not SchemaObject foundSchema)
                    throw new InvalidOperationException($"unexpected schema which is not object in store. key:${schema.SchemaKey}, type:${schema.SchemaType}");
                return foundSchema;
            }

            SchemaObject r = new SchemaObject(csharpType.GenerateSeSchemaKey());
            SchemaStore.Current.AddSchema(r);
            if (csharpType.Implementation is SchemaObjectType || csharpType.Implementation is ModelTypeProvider)
            {
                var imp = (SerializableObjectType)csharpType.Implementation;

                r.IsStruct = imp.IsStruct;
                r.InheritFrom = imp.Inherits == null ? null : imp.Inherits.CreateSeTypeDesc();
                if (imp.Discriminator != null)
                {
                    // TODO: do we have multi-level inherit for discriminator? only consider one level here and support multi-level when we find the case exists
                    if (imp.Discriminator.Property.ValueType.IsFrameworkType && imp.Discriminator.Property.ValueType.FrameworkType == typeof(string))
                    {
                        if (imp.Discriminator.Implementations.Length > 0)
                        {
                            List<SchemaEnumValue> keys = new List<SchemaEnumValue>();
                            r.IsDiscriminatorBase = true;
                            //this.DiscriminatorProperty = new MgmtExplorerSchemaProperty(imp.Discriminator.Property);
                            r.InheritBy = imp.Discriminator.Implementations.Select(m =>
                            {
                                var childImp = (SchemaObjectType)m.Type.Implementation;
                                string? key = childImp.Discriminator?.Value?.Value as string;
                                if (key == null)
                                    throw new InvalidOperationException("Can't find string key for implemenation of discriminator: " + m.Type.Name);
                                keys.Add(new SchemaEnumValue()
                                {
                                    Value = key,
                                    Description = childImp.Description ?? "",
                                    InternalValue = key,
                                });
                                return m.Type.CreateSeTypeDesc();
                            }).ToList();
                            string keyNamespace = $"{csharpType.Namespace}.SdkExplorerDiscriminator";
                            string keyTypeName = $"{imp.Discriminator.Property.Declaration.Name}Enum";
                            string keySchemaKey = $"{keyNamespace}.{keyTypeName}";
                            var keysEnumSchema = new SchemaEnum(keySchemaKey)
                            {
                                Description = imp.Description ?? "",
                                Values = keys,
                            };
                            SchemaStore.Current.AddSchema(keysEnumSchema);
                            r.DiscriminatorProperty = new SchemaProperty()
                            {
                                Accessibility = "internal",
                                IsReadonly = true,
                                IsRequired = true,
                                Name = imp.Discriminator.Property.Declaration.Name,
                                SerializerPath = imp.Discriminator.Property.SchemaProperty?.SerializedName ?? imp.Discriminator.Property.Declaration.Name,
                                Type = TypeDesc.CreateEnumType(keyTypeName, keyNamespace, false, keysEnumSchema),
                            };
                        }
                        if (imp.Discriminator?.Value != null)
                        {
                            string? key = imp.Discriminator?.Value?.Value as string;
                            if (key == null)
                                throw new InvalidOperationException("Discriminator's key value is null: " + csharpType.Name);
                            r.DiscriminatorKey = key;
                        }
                    }
                    else if (imp.Discriminator.Property.ValueType.Implementation is EnumType)
                    {
                        var keyEnum = imp.Discriminator.Property.ValueType.Implementation as EnumType;
                        if (keyEnum == null)
                            throw new InvalidOperationException("discriminator's type property is not enum");
                        if (imp.Discriminator.Implementations.Length > 0)
                        {
                            r.IsDiscriminatorBase = true;
                            r.DiscriminatorProperty = imp.Discriminator.Property.CreateSeSchemaProperty();
                            r.InheritBy = imp.Discriminator.Implementations.Select(m =>
                            {
                                var childImp = (SchemaObjectType)m.Type.Implementation;
                                EnumTypeValue? etv = childImp.Discriminator?.Value?.Value as EnumTypeValue;
                                string? key = etv?.Value.Value as string;
                                if (key == null)
                                    throw new InvalidOperationException("Can't find key for implemenation of discriminator: " + m.Type.Name);
                                if (keyEnum.Values.FirstOrDefault(mm => key == (mm.Value.Value as string)) == null)
                                    throw new InvalidOperationException("Can't find key for implementation in type enum: " + key);
                                return m.Type.CreateSeTypeDesc();
                            }).ToList();
                            if (r.InheritBy.Count != keyEnum.Values.Count)
                                throw new InvalidOperationException($"implementation and key enum count mismatch: impl={string.Join('|', r.InheritBy.Select(m => m.Name))}, keyEnum={string.Join('|', keyEnum.Values.Select(m => m.Value.Value ?? ""))}");
                        }
                        if (imp.Discriminator?.Value != null)
                        {
                            EnumTypeValue? etv = imp.Discriminator?.Value?.Value as EnumTypeValue;
                            string? key = etv?.Value.Value as string;
                            if (key == null)
                                throw new InvalidOperationException("Discriminator's key value is null: " + csharpType.Name);
                            r.DiscriminatorKey = key;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"Unexpected type as discriminator type={csharpType.Name}, discriminator property type = {imp.Discriminator.Property.ValueType.Name}");
                    }
                }
                r.Description = imp.Description ?? "";
                r.InitializationConstructor = imp.InitializationConstructor.Signature.Modifiers == Output.Models.MethodSignatureModifiers.Public ? imp.InitializationConstructor.CreateSeSchemaMethod() : null;
                r.SerializationConstructor = imp.SerializationConstructor.Signature.Modifiers == Output.Models.MethodSignatureModifiers.Public ? imp.SerializationConstructor.CreateSeSchemaMethod() : null;

                r.Properties = imp.Properties.GenerateSchemaProperties(r.GetDefaultConstructor());
            }
            else if (csharpType.Implementation is SystemObjectType)
            {
                var imp = (SystemObjectType)csharpType.Implementation;

                r.IsStruct = false;
                r.InheritFrom = imp.Inherits == null ? null : imp.Inherits.CreateSeTypeDesc();
                r.InheritBy = new List<TypeDesc>();
                r.Description = ""; // not supported in SystemObjectType...
                r.InitializationConstructor = imp.InitializationConstructor.Signature.Modifiers == Output.Models.MethodSignatureModifiers.Public ? imp.InitializationConstructor.CreateSeSchemaMethod() : null;
                r.SerializationConstructor = imp.SerializationConstructor.Signature.Modifiers == Output.Models.MethodSignatureModifiers.Public ? imp.SerializationConstructor.CreateSeSchemaMethod() : null;
                r.Properties = imp.Properties.GenerateSchemaProperties(r.GetDefaultConstructor());

                UpdateSystemObjectType(r, imp);
            }
            else
                throw new ArgumentException("Unsupported type to create schema for non-frameworktype: " + csharpType.Implementation.GetType().FullName);

            return r;
        }

        private static List<SchemaProperty> GenerateSchemaProperties(this ObjectTypeProperty[] props, SchemaMethod? ctor)
        {
            var newProps = props.Select(p => p.FlattenedProperty ?? p)
                .Where(p => p.Declaration.Accessibility == "public" &&
                    (!p.IsReadOnly || (p.Declaration.Type.IsFrameworkType && (TypeFactory.IsReadWriteList(p.Declaration.Type) || TypeFactory.IsReadWriteDictionary(p.Declaration.Type)))))
                .Select(p => p.CreateSeSchemaProperty(isWritableThroughCtor: false));
            if (ctor != null)
            {
                // TODO: Is it enough to use default ctor to figure out all readonly parameters but still writable through ctor? keep things simple as it is now and improve when needed
                var throughCtorProps = props.Select(p => p.FlattenedProperty ?? p)
                    .Where(p => p.IsReadOnly && ctor.HasParameter(p.GetSerializerNameOrName()))
                    .Select(p => p.CreateSeSchemaProperty(isWritableThroughCtor: true));
                newProps = newProps.Concat(throughCtorProps);
            }
            return newProps.ToList();
        }

        private static bool IsConstructorMatch(ConstructorInfo a, SchemaMethod b)
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
        private static void UpdateSystemObjectType(SchemaObject schemaObject, SystemObjectType sot)
        {
            var systemType = sot.SystemType;

            // some ctor should be Internal instead of public, seems a bug at SystemObjectType.cs line 136, the ctor can be NonPublic (not sure whehter we should fix there, so fix here as a temp workaround)
            var ctors = systemType.GetConstructors();
            if (schemaObject.InitializationConstructor != null)
            {
                if (ctors.FirstOrDefault(ctor => IsConstructorMatch(ctor, schemaObject.InitializationConstructor)) == null)
                    schemaObject.InitializationConstructor = null;
            }
            if (schemaObject.SerializationConstructor != null)
            {
                if (ctors.FirstOrDefault(ctor => IsConstructorMatch(ctor, schemaObject.SerializationConstructor)) == null)
                    schemaObject.SerializationConstructor = null;
            }

            foreach (var prop in schemaObject.Properties)
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
        }

        private static SchemaNone GetOrCreateSeSchemaNone(this CSharpType csharpType, string reason)
        {
            var schema = csharpType.GetSchemaFromStore();
            if (schema != null)
            {
                if (schema is not SchemaNone foundSchema)
                    throw new InvalidOperationException($"unexpected schema which is not SchemaNone in store. key:${schema.SchemaKey}, type:${schema.SchemaType}");
                return foundSchema;
            }

            SchemaNone r = new SchemaNone(csharpType.GenerateSeSchemaKey())
            {
                Reason = reason,
            };
            SchemaStore.Current.AddSchema(r);
            return r;
        }

        private static SchemaProperty CreateSeSchemaProperty(this ObjectTypeProperty prop, bool isWritableThroughCtor = false)
        {
            SchemaProperty r = new SchemaProperty()
            {
                Accessibility = prop.Declaration.Accessibility,
                // TODO: CombinedName may be used instead of name
                // TODO: there is extra logic to handle the single property object in sdk codegen. add handle for it when we hit the case
                // this.CombinedName = this.Name;
                Name = prop.Declaration.Name,
                Description = prop.Description,
                Type = prop.Declaration.Type.CreateSeTypeDesc(),
                IsRequired = prop.SchemaProperty?.IsRequired ?? false,
                IsReadonly = prop.IsReadOnly,
                IsWritableThroughCtor = isWritableThroughCtor,
            };
            if (prop is FlattenedObjectTypeProperty fp)
            {
                var list = fp.BuildHierarchyStack().ToList();
                list.Reverse();
                r.SerializerPath = string.Join("/", list.Select(s => s.GetSerializerNameOrName()));
            }
            else
            {
                r.SerializerPath = prop.GetSerializerNameOrName();
            }
            return r;
        }

        private static SchemaMethodParameter CreateSeSchemaMethodParameter(this Parameter param, ObjectTypeProperty? initializedProp)
        {
            SchemaMethodParameter r = new SchemaMethodParameter()
            {
                Name = param.Name,
                RelatedPropertySerializerPath = initializedProp == null ? null : initializedProp.CreateSeSchemaProperty().SerializerPath,
                Type = param.Type.CreateSeTypeDesc(),
                IsOptional = param.IsOptionalInSignature,
                DefaultValue = param.DefaultValue.ToString(),
                Description = param.Description ?? ""
            };
            return r;
        }

        private static SchemaMethod CreateSeSchemaMethod(this ObjectTypeConstructor ctor)
        {
            var r = new SchemaMethod();
            r.Name = ctor.Signature.Name;
            r.MethodParameters = ctor.Signature.Parameters.Select(p => p.CreateSeSchemaMethodParameter(ctor.FindPropertyInitializedByParameter(p))).ToList();
            return r;
        }

        private static SchemaEnumValue CreateSeSchemaEnumValue(this EnumTypeValue enumValue)
        {
            SchemaEnumValue r = new SchemaEnumValue();
            r.Value = enumValue.Declaration.Name;
            r.InternalValue = enumValue.Value.Value!.ToString();
            r.Description = enumValue.Description;
            return r;
        }

        private static SchemaEnum GetOrCreateSeSchemaEnum(this CSharpType csharpType)
        {
            var schema = csharpType.GetSchemaFromStore();
            if (schema != null)
            {
                if (schema is not SchemaEnum foundSchema)
                    throw new InvalidOperationException($"unexpected schema which is not enum in store. key:${schema.SchemaKey}, type:${schema.SchemaType}");
                return foundSchema;
            }

            SchemaEnum r = new SchemaEnum(csharpType.GenerateSeSchemaKey());
            var imp = (EnumType)csharpType.Implementation;
            r.Values = imp.Values.Select(v => v.CreateSeSchemaEnumValue()).ToList();

            SchemaStore.Current.AddSchema(r);
            return r;
        }

        private static string GenerateSeSchemaKey(this CSharpType type)
        {
            return type.GetFullName(true, false);
        }

        private static void SetSchemaForCtorWithOneStringParam(SchemaObject schema, CSharpType csharpType, string description, string paramName, string paramDescription)
        {
            schema.Description = description;
            schema.InitializationConstructor = null;
            schema.SerializationConstructor = new SchemaMethod()
            {
                Name = csharpType.GetFullName(false /*includeNamespace*/, false /*includeNullable*/),
                MethodParameters = new List<SchemaMethodParameter>()
                        {
                            new SchemaMethodParameter()
                            {
                                Name = paramName,
                                DefaultValue = null,
                                Description = paramDescription,
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = (new CSharpType(typeof(string))).CreateSeTypeDesc(),
                            }
                        }
            };
        }

        private static void SetSchemaForStaticCreateMethodWithOneStringParam(SchemaObject schema, CSharpType cSharpType, string methodName, string description, string paramName, string paramDescription)
        {
            schema.Description = description;
            schema.InitializationConstructor = null;
            schema.StaticCreateMethod = new SchemaMethod()
            {
                Name = methodName,
                MethodParameters = new List<SchemaMethodParameter>()
                        {
                            new SchemaMethodParameter()
                            {
                                Name = paramName,
                                DefaultValue = null,
                                Description = paramDescription,
                                IsOptional = false,
                                RelatedPropertySerializerPath = paramName,
                                Type = new CSharpType(typeof(string)).CreateSeTypeDesc()
                            }
                        }
            };
        }

        private static List<SchemaEnumValue> generateEnumValues(string[] array)
        {
            return array.Select(s => new SchemaEnumValue()
            {
                Value = s,
                Description = s,
            }).ToList();
        }

        private static SchemaObject? GetOrCreateSchemaObjectForFrameworkType(this CSharpType csharpType)
        {
            if (csharpType.IsFrameworkType == false)
                throw new InvalidOperationException("type is not frameworktype: " + csharpType.Name);

            var found = csharpType.GetSchemaFromStore();
            if (found != null)
            {
                if (found is not SchemaObject so)
                    throw new InvalidOperationException($"Unexpected Schema type. key: {found.SchemaKey}, type: {found.SchemaType}");
                return so;
            }

            var schema = new SchemaObject(csharpType.GenerateSeSchemaKey());
            schema.IsStruct = false;
            schema.IsEnum = false;
            schema.InheritFrom = null;
            schema.InheritBy = new List<TypeDesc>();
            schema.Description = "";
            schema.Properties = new List<SchemaProperty>();
            schema.InitializationConstructor = new SchemaMethod()
            {
                Name = csharpType.GetFullName(false /*includeNamespace*/, false /*includeNullable*/),
                MethodParameters = new List<SchemaMethodParameter>(),
            };
            schema.SerializationConstructor = null;

            if (csharpType.FrameworkType == typeof(Azure.ETag))
            {
                schema.IsStruct = true;
                SetSchemaForCtorWithOneStringParam(schema, csharpType, "Represents an HTTP ETag.", "etag", "The string value of the ETag.");
            }
            else if (csharpType.FrameworkType == typeof(ResourceIdentifier))
            {
                SetSchemaForCtorWithOneStringParam(schema, csharpType, "An Azure Resource Manager resource identifier.", "resourceId", "Initializes a new instance of the ResourceIdentifier class.");
            }
            else if (csharpType.FrameworkType == typeof(Uri))
            {
                SetSchemaForCtorWithOneStringParam(schema, csharpType, "Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.", "uriString", "A string that identifies the resource to be represented by the Uri instance. Note that an IPv6 address in string form must be enclosed within brackets. For example, \"http://[2607:f8b0:400d:c06::69]\".");
            }
            else if (csharpType.FrameworkType == typeof(Guid))
            {
                SetSchemaForCtorWithOneStringParam(schema, csharpType, "Represents a globally unique identifier (GUID).", "g", "A string that contains a GUID in one of the following formats(\"d\" represents a hexadecimal digit whose case is ignored):\ndddddddddddddddddddddddddddddddd\ndddddddd-dddd-dddd-dddd-dddddddddddd\n{dddddddd-dddd-dddd-dddd-dddddddddddd}\n(dddddddd-dddd-dddd-dddd-dddddddddddd)\n{0xdddddddd, 0xdddd, 0xdddd,{0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd,0xdd}}");
            }
            else if (csharpType.FrameworkType == typeof(IPAddress))
            {
                SetSchemaForStaticCreateMethodWithOneStringParam(schema, csharpType, "Parse",
                    "Provides an Internet Protocol (IP) address.", "ipString", "A string that contains an IP address in dotted-quad notation for IPv4 and in colon-hexadecimal notation for IPv6.");
            }
            else if (csharpType.FrameworkType == typeof(TimeSpan))
            {
                schema.Description = "Represents a time interval.";
                schema.SerializationConstructor = new SchemaMethod()
                {
                    Name = csharpType.GetFullName(false /*includeNamespace*/, false /*includeNullable*/),
                    MethodParameters = new List<SchemaMethodParameter>()
                        {
                            new SchemaMethodParameter()
                            {
                                Name = "days",
                                DefaultValue = null,
                                Description = "Number of days.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = (new CSharpType(typeof(int))).CreateSeTypeDesc()
                            },
                            new SchemaMethodParameter()
                            {
                                Name = "hours",
                                DefaultValue = null,
                                Description = "Number of hours.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = (new CSharpType(typeof(int))).CreateSeTypeDesc()
                            },
                            new SchemaMethodParameter()
                            {
                                Name = "minutes",
                                DefaultValue = null,
                                Description = "Number of minutes.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = (new CSharpType(typeof(int))).CreateSeTypeDesc()
                            },
                            new SchemaMethodParameter()
                            {
                                Name = "seconds",
                                DefaultValue = null,
                                Description = "Number of seconds.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = (new CSharpType(typeof(int))).CreateSeTypeDesc()
                            },
                            new SchemaMethodParameter()
                            {
                                Name = "milliseconds",
                                DefaultValue = null,
                                Description = "Number of milliseconds.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = (new CSharpType(typeof(int))).CreateSeTypeDesc()
                            },
                        }
                };
            }
            else if (csharpType.FrameworkType == typeof(ResourceType))
            {
                var desc = "Structure representing a resource type. See < a href =\"/en-us/azure/azure-resource-manager/management/resource-providers-and-types\" data-linktype=\"absolute-path\">https://docs.microsoft.com/en-us/azure/azure-resource-manager/management/resource-providers-and-types</a> for more info.";
                SetSchemaForCtorWithOneStringParam(schema, csharpType, desc, "resourceType", "The resource type string to convert");
            }
            else if (csharpType.FrameworkType == typeof(ExtendedLocationType))
            {
                SetSchemaForCtorWithOneStringParam(schema, csharpType, "The extended location type.", "value", "");
                schema.IsEnum = true;
                // prefer hardcode to reflection to avoid unexpected change which will be hard to maintain.
                schema.EnumValues = generateEnumValues(new string[]
                {
                    "EdgeZone",
                });
            }
            else if (csharpType.FrameworkType == typeof(UserAssignedIdentity))
            {
                schema.Description = "User assigned identity properties";
            }
            else if (csharpType.FrameworkType == typeof(ContentType))
            {
                SetSchemaForCtorWithOneStringParam(schema, csharpType, "Represents content type.", "contentType", "The content type string.");
                schema.IsEnum = true;
                // prefer hardcode to reflection to avoid unexpected change which will be hard to maintain.
                schema.EnumValues = generateEnumValues(new string[]
                {
                    "ApplicationJson",
                    "ApplicationOctetStream",
                    "TextPlain",
                });
            }
            else if (csharpType.FrameworkType == typeof(ManagedServiceIdentityType))
            {
                SetSchemaForCtorWithOneStringParam(schema, csharpType, "Type of managed service identity (where both SystemAssigned and UserAssigned types are allowed).", "value", "");
                schema.IsEnum = true;
                // prefer hardcode to reflection to avoid unexpected change which will be hard to maintain.
                schema.EnumValues = generateEnumValues(new string[]
                {
                    "None",
                    "SystemAssigned",
                    "SystemAssignedUserAssigned",
                    "UserAssigned",
                });
            }
            else if (csharpType.FrameworkType == typeof(RequestMethod))
            {
                SetSchemaForCtorWithOneStringParam(schema, csharpType, "Represents HTTP methods sent as part of a Request.", "method", "The method to use.");
                schema.IsEnum = true;
                // prefer hardcode to reflection to avoid unexpected change which will be hard to maintain.
                schema.EnumValues = generateEnumValues(new string[]
                {
                    "Delete",
                    "Get",
                    "Head",
                    "Method",
                    "Patch",
                    "Post",
                    "Put",
                });
            }
            else if (csharpType.FrameworkType == typeof(AzureLocation))
            {
                // type honestily, portal can expose it as enum for better user experience.
                schema.Description = "Represents an Azure geography region where supported resource providers live.";
                schema.SerializationConstructor = new SchemaMethod()
                {
                    Name = csharpType.GetFullName(false /*includeNamespace*/, false /*includeNullable*/),
                    MethodParameters = new List<SchemaMethodParameter>()
                        {
                            new SchemaMethodParameter()
                            {
                                Name = "name",
                                DefaultValue = null,
                                Description = "The location name.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = new CSharpType(typeof(string)).CreateSeTypeDesc()
                            },
                            new SchemaMethodParameter()
                            {
                                Name = "displayName",
                                DefaultValue = null,
                                Description = "The display name of the location.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = new CSharpType(typeof(string)).CreateSeTypeDesc()
                            }
                        }
                };
                schema.IsEnum = true;
                schema.EnumValues = generateEnumValues(
                    // prefer hardcode to reflection to avoid unexpected change which will be hard to maintain.
                    new string[] {"AustraliaCentral",
                                  "AustraliaCentral2",
                                  "AustraliaEast",
                                  "AustraliaSoutheast",
                                  "BrazilSouth",
                                  "BrazilSoutheast",
                                  "CanadaCentral",
                                  "CanadaEast",
                                  "CentralIndia",
                                  "CentralUS",
                                  "ChinaEast",
                                  "ChinaEast2",
                                  "ChinaNorth",
                                  "ChinaNorth2",
                                  "EastAsia",
                                  "EastUS",
                                  "EastUS2",
                                  "FranceCentral",
                                  "FranceSouth",
                                  "GermanyCentral",
                                  "GermanyNorth",
                                  "GermanyNorthEast",
                                  "GermanyWestCentral",
                                  "JapanEast",
                                  "JapanWest",
                                  "KoreaCentral",
                                  "KoreaSouth",
                                  "NorthCentralUS",
                                  "NorthEurope",
                                  "NorwayEast",
                                  "NorwayWest",
                                  "SouthAfricaNorth",
                                  "SouthAfricaWest",
                                  "SouthCentralUS",
                                  "SoutheastAsia",
                                  "SouthIndia",
                                  "SwitzerlandNorth",
                                  "SwitzerlandWest",
                                  "UAECentral",
                                  "UAENorth",
                                  "UKSouth",
                                  "UKWest",
                                  "USDoDCentral",
                                  "USDoDEast",
                                  "USGovArizona",
                                  "USGovIowa",
                                  "USGovTexas",
                                  "USGovVirginia",
                                  "WestCentralUS",
                                  "WestEurope",
                                  "WestIndia",
                                  "WestUS",
                                  "WestUS2" });
            }
            else
            {
                return null;
                //throw new InvalidOperationException("Unsupported FrameworkType: " + csharpType.FrameworkType.FullName);
            }
            SchemaStore.Current.AddSchema(schema);
            return schema;
        }

    }
}
