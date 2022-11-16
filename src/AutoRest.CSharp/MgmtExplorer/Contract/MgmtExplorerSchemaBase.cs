// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Reflection.Emit;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.MgmtExplorer.Autorest;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerSchemaBase
    {
        public string SchemaKey { get; set; } = string.Empty;
        public string SchemaType { get; set; } = string.Empty;

        protected MgmtExplorerSchemaBase(string schemaKey, string schemaType)
        {
            this.SchemaKey = schemaKey;
            this.SchemaType = schemaType;
        }

        public MgmtExplorerSchemaBase()
        {

        }

        internal static string generateKey(CSharpType type)
        {
            return type.GetFullName(true, false);
        }
    }
}
