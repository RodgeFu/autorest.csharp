// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.CSharp.Output.Models.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerSchemaProperty
    {
        public string? Accessibility { get; set; }
        public MgmtExplorerCSharpType? Type { get; set; }
        public string? Name { get; set; }
        public string? SerializerPath { get; set; }
        public bool IsRequired { get; set; }
        public bool IsReadonly { get; set; }

        public string? CombinedName { get; set; }
        public string? CombinedSerializerPath { get; set; }

        public MgmtExplorerSchemaProperty()
        {
        }

        internal MgmtExplorerSchemaProperty(ObjectTypeProperty prop)
        {
            this.Accessibility = prop.Declaration.Accessibility;
            // TODO: CombinedName may be used instead of name
            // TODO: there is extra logic to handle the single property object in sdk codegen. add handle for it when we hit the case
            // this.CombinedName = this.Name;
            this.Name = prop.Declaration.Name;
            this.SerializerPath = prop.SchemaProperty?.SerializedName ?? this.Name;
            this.Type = new MgmtExplorerCSharpType(prop.Declaration.Type);
            this.IsRequired = prop.SchemaProperty?.IsRequired ?? false;
            this.IsReadonly = prop.IsReadOnly;

        }
    }
}
