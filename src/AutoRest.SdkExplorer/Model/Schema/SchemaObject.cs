// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using AutoRest.SdkExplorer.Model.Code;
using YamlDotNet.Serialization;

namespace AutoRest.SdkExplorer.Model.Schema
{
    public class SchemaObject : SchemaBase
    {
        public const string SCHEMA_TYPE = "OBJECT_SCHEMA";

        public List<SchemaProperty> Properties { get; set; } = new List<SchemaProperty>();
        public TypeDesc? InheritFrom { get; set; }
        // TODO: discriminator support
        public List<TypeDesc> InheritBy { get; set; } = new List<TypeDesc>();
        public SchemaMethod? InitializationConstructor { get; set; }
        public SchemaMethod? SerializationConstructor { get; set; }
        public SchemaMethod? StaticCreateMethod { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsStruct { get; set; }
        public bool IsEnum { get; set; }
        public List<SchemaEnumValue> EnumValues { get; set; } = new List<SchemaEnumValue>();
        public string DiscriminatorKey { get; set; } = string.Empty;
        public bool IsDiscriminatorBase { get; set; } = false;
        public SchemaProperty? DiscriminatorProperty { get; set; }

        [YamlIgnore, JsonIgnore]
        public SchemaMethod? DefaultConstructor => this.SerializationConstructor ?? this.InitializationConstructor ?? this.StaticCreateMethod ?? null;
        /// <summary>
        /// Include properties from inherit
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SchemaProperty> GetAllProperties(SchemaStore? store = null)
        {
            foreach (var sp in this.Properties)
                yield return sp;
            var parent = this.InheritFrom?.GetSchema(store);
            if (parent != null)
            {
                if (parent is not SchemaObject so)
                    throw new InvalidOperationException("Object Schema is expected for parent object: " + this.SchemaKey);
                foreach (var r in so.GetAllProperties(store))
                    yield return r;
            }
        }

        private SchemaObject()
            : base("OnlyForYamlDeserializor", SCHEMA_TYPE)
        {
        }

        [JsonConstructor]
        public SchemaObject(string schemaKey)
            : base(schemaKey, SCHEMA_TYPE)
        {
        }

        public bool IsDefaultConstructorParameter(string parameterSerializerName)
        {
            if (this.DefaultConstructor == null)
                return false;
            else
                return this.DefaultConstructor.HasParameter(parameterSerializerName);
        }

    }
}
