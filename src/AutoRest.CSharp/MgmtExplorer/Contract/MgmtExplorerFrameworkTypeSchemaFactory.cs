// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.MgmtExplorer.Autorest;
using Azure.Core;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    internal static class MgmtExplorerFrameworkTypeSchemaFactory
    {
        private static void SetSchemaForCtorWithOneStringParam(MgmtExplorerSchemaObject schema, CSharpType csharpType, string description, string paramName, string paramDescription)
        {
            schema.Description = description;
            schema.SerializationConstructor = new MgmtExplorerSchemaConstructor()
            {
                Name = csharpType.GetFullName(true /*includeNamespace*/, false /*includeNullable*/),
                BaseConstructor = null,
                MethodParameters = new List<MgmtExplorerSchemaConstructorParameter>()
                        {
                            new MgmtExplorerSchemaConstructorParameter()
                            {
                                Name = paramName,
                                DefaultValue = null,
                                Description = paramDescription,
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = new MgmtExplorerCSharpType(new CSharpType(typeof(string))),
                            }
                        }
            };
        }

        private static List<MgmtExplorerSchemaEnumValue> generateEnumValues(string[] array)
        {
            return array.Select(s => new MgmtExplorerSchemaEnumValue()
            {
                Value = s,
                Description = s,
            }).ToList();
        }

        public static MgmtExplorerSchemaObject? GetSchema(CSharpType csharpType)
        {
            if (csharpType == null)
                throw new ArgumentNullException(nameof(csharpType));
            if (csharpType.IsFrameworkType == false)
                throw new InvalidOperationException("type is not frameworktype: " + csharpType.Name);

            var schema = new MgmtExplorerSchemaObject(csharpType);
            schema.IsStruct = false;
            schema.IsEnum = false;
            schema.InheritFrom = null;
            schema.InheritBy = new List<MgmtExplorerCSharpType>();
            schema.Description = "";
            schema.Properties = new List<MgmtExplorerSchemaProperty>();
            schema.InitializationConstructor = new MgmtExplorerSchemaConstructor()
            {
                Name = csharpType.GetFullName(true /*includeNamespace*/, false /*includeNullable*/),
                BaseConstructor = null,
                MethodParameters = new List<MgmtExplorerSchemaConstructorParameter>(),
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
            else if (csharpType.FrameworkType == typeof(TimeSpan))
            {
                schema.Description = "Represents a time interval.";
                schema.SerializationConstructor = new MgmtExplorerSchemaConstructor()
                {
                    Name = csharpType.GetFullName(true /*includeNamespace*/, false /*includeNullable*/),
                    BaseConstructor = null,
                    MethodParameters = new List<MgmtExplorerSchemaConstructorParameter>()
                        {
                            new MgmtExplorerSchemaConstructorParameter()
                            {
                                Name = "days",
                                DefaultValue = null,
                                Description = "Number of days.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = new MgmtExplorerCSharpType(new CSharpType(typeof(int))),
                            },
                            new MgmtExplorerSchemaConstructorParameter()
                            {
                                Name = "hours",
                                DefaultValue = null,
                                Description = "Number of hours.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = new MgmtExplorerCSharpType(new CSharpType(typeof(int))),
                            },
                            new MgmtExplorerSchemaConstructorParameter()
                            {
                                Name = "minutes",
                                DefaultValue = null,
                                Description = "Number of minutes.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = new MgmtExplorerCSharpType(new CSharpType(typeof(int))),
                            },
                            new MgmtExplorerSchemaConstructorParameter()
                            {
                                Name = "seconds",
                                DefaultValue = null,
                                Description = "Number of seconds.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = new MgmtExplorerCSharpType(new CSharpType(typeof(int))),
                            },
                            new MgmtExplorerSchemaConstructorParameter()
                            {
                                Name = "milliseconds",
                                DefaultValue = null,
                                Description = "Number of milliseconds.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = new MgmtExplorerCSharpType(new CSharpType(typeof(int))),
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
                SetSchemaForCtorWithOneStringParam(schema, csharpType, "The reference to a user assigned identity associated with the Batch pool which a compute node will use.", "resourceId", "The ARM resource id of the user assigned identity");
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
                schema.SerializationConstructor = new MgmtExplorerSchemaConstructor()
                {
                    Name = csharpType.GetFullName(true /*includeNamespace*/, false /*includeNullable*/),
                    BaseConstructor = null,
                    MethodParameters = new List<MgmtExplorerSchemaConstructorParameter>()
                        {
                            new MgmtExplorerSchemaConstructorParameter()
                            {
                                Name = "name",
                                DefaultValue = null,
                                Description = "The location name.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = new MgmtExplorerCSharpType(new CSharpType(typeof(string))),
                            },
                            new MgmtExplorerSchemaConstructorParameter()
                            {
                                Name = "displayName",
                                DefaultValue = null,
                                Description = "The display name of the location.",
                                IsOptional = false,
                                RelatedPropertySerializerPath = csharpType.Name,
                                Type = new MgmtExplorerCSharpType(new CSharpType(typeof(string))),
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
            return schema;
        }
    }
}
