// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using AutoRest.CSharp.Output.Models.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerSchemaEnumValue
    {
        public string? Value { get; set; }
        public string? InternalValue { get; set; }
        public string Description { get; set; } = string.Empty;

        public MgmtExplorerSchemaEnumValue()
        {

        }

        internal MgmtExplorerSchemaEnumValue(EnumTypeValue enumValue)
        {
            this.Value = enumValue.Declaration.Name;
            this.InternalValue = enumValue.Value.Value!.ToString();
            this.Description = enumValue.Description;
        }
    }
}
