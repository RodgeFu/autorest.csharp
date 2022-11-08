// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerSchemaEnum : MgmtExplorerSchemaBase
    {
        public List<MgmtExplorerSchemaEnumValue> Values { get; set; } = new List<MgmtExplorerSchemaEnumValue>();
        public string Description { get; set; } = string.Empty;

        public MgmtExplorerSchemaEnum()
            :base()
        {

        }
    }
}
