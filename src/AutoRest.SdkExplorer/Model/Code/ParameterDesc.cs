// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using AutoRest.SdkExplorer.Model.Example;
using AutoRest.SdkExplorer.Model.Schema;
using AutoRest.SdkExplorer.Model.Hint;

namespace AutoRest.SdkExplorer.Model.Code
{
    public class ParameterDesc : PlaceHolderDesc
    {
        public string? ModelName { get; set; }
        public string? SerializerName { get; set; }
        public TypeDesc? Type { get; set; }
        public string? Description { get; set; }
        public string? RequestPath { get; set; }
        public string? DefaultValue { get; set; }
        public string? Source { get; set; }
        public string? SourceArg { get; set; }
        public bool IsInPropertyBag { get; set; }

        public ParameterDesc()
            : base()
        {
        }

        public IEnumerable<FieldHint> GetFieldHints(string apiName, ExampleDesc ex, SchemaStore? schemaStore = null)
        {
            if (ex.ExampleValues.TryGetValue(this.SerializerName!, out ExampleValueDesc? found))
                return this.Type!.GetFieldHints($"{apiName}.{this.SerializerName!}", ex.ExampleName!, found!, schemaStore);
            return new List<FieldHint>();
        }
    }
}
