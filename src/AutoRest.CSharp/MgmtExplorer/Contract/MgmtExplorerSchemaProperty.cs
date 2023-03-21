﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using AutoRest.CSharp.Output.Models.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerSchemaProperty
    {
        public string? Accessibility { get; set; }
        public MgmtExplorerCSharpType? Type { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? SerializerPath { get; set; }
        public bool IsRequired { get; set; }
        public bool IsReadonly { get; set; }

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
            this.Description = prop.Description;
            this.Type = new MgmtExplorerCSharpType(prop.Declaration.Type);
            this.IsRequired = prop.SchemaProperty?.IsRequired ?? false;
            this.IsReadonly = prop.IsReadOnly;

            if (prop is FlattenedObjectTypeProperty fp)
            {
                var list = fp.BuildHierarchyStack().ToList();
                list.Reverse();
                this.SerializerPath = string.Join("/", list.Select(s => s.SchemaProperty?.SerializedName ?? s.Declaration.Name));
            }
            else
            {
                this.SerializerPath = prop.SchemaProperty?.SerializedName ?? this.Name;
            }
        }
    }
}
