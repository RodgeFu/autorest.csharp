// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerSchemaObject
    {
        public List<MgmtExplorerSchemaProperty> Properties { get; set; } = new List<MgmtExplorerSchemaProperty>();
        public MgmtExplorerCSharpType? InheritFrom { get; set; }
        // TODO: discriminator support
        public List<MgmtExplorerCSharpType> InheritBy { get; set; } = new List<MgmtExplorerCSharpType>();
        public MgmtExplorerSchemaConstructor? InitializationConstructor { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsStruct { get; set; }

        public MgmtExplorerSchemaObject()
        {

        }
    }
}
