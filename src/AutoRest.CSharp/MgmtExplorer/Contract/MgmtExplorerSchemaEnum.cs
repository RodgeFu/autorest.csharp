// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Input;
using AutoRest.CSharp.Output.Models.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerSchemaEnum : MgmtExplorerSchemaBase
    {
        public const string SCHEMA_TYPE = "ENUM_SCHEMA";

        public List<MgmtExplorerSchemaEnumValue> Values { get; set; } = new List<MgmtExplorerSchemaEnumValue>();
        public string Description { get; set; } = string.Empty;

        internal MgmtExplorerSchemaEnum(CSharpType csharpType)
            : base(generateKey(csharpType), SCHEMA_TYPE)
        {
            var imp = (EnumType)csharpType.Implementation;
            this.Values = imp.Values.Select(v => new MgmtExplorerSchemaEnumValue(v)).ToList();
        }

        public MgmtExplorerSchemaEnum()
            : base()
        {

        }
    }
}
