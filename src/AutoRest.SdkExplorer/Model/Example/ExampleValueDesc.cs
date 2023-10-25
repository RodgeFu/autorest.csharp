// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace AutoRest.SdkExplorer.Model.Example
{
    public class ExampleValueDesc
    {
        public string? SerializerName { get; set; }
        public string? ModelName { get; set; }
        /// <summary>
        /// Refer to AllSchemaTypes in autorest.csharp
        /// </summary>
        public string? SchemaType { get; set; }
        public string? CSharpName { get; set; }
        public string? Description { get; set; }
        public string? RawValue { get; set; }
        public Dictionary<string, ExampleValueDesc>? PropertyValues { get; set; }
        public List<ExampleValueDesc>? ArrayValues { get; set; }

        public ExampleValueDesc()
        { }

        public bool HasRawValue() => RawValue != null;
        public bool HasPropertyValues() => this.PropertyValues != null;
        public bool HasArrayValues() => this.ArrayValues != null;
    }
}
