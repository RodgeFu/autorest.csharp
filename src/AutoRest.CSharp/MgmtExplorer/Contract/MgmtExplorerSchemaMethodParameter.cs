// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.CSharp.Output.Models.Shared;
using AutoRest.CSharp.Output.Models.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerSchemaMethodParameter
    {
        public string? Name { get; set; }
        public string? RelatedPropertySerializerPath { get; set; }
        public MgmtExplorerCSharpType? Type { get; set; }
        public bool IsOptional { get; set; }
        public string? DefaultValue { get; set; }
        public string Description { get; set; } = string.Empty;

        public MgmtExplorerSchemaMethodParameter()
        {

        }

        internal MgmtExplorerSchemaMethodParameter(Parameter param, ObjectTypeProperty? initializedProp)
        {
            this.Name = param.Name;
            this.RelatedPropertySerializerPath = initializedProp == null ? null : new MgmtExplorerSchemaProperty(initializedProp).SerializerPath;
            this.Type = new MgmtExplorerCSharpType(param.Type);
            this.IsOptional = param.IsOptionalInSignature;
            this.DefaultValue = param.DefaultValue.ToString();
            this.Description = param.Description ?? "";
        }
    }
}
