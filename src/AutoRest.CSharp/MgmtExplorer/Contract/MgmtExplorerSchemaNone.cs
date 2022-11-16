// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.CSharp.Generation.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerSchemaNone : MgmtExplorerSchemaBase
    {
        public const string SCHEMA_TYPE = "NONE_SCHEMA";

        public string Reason { get; set; } = string.Empty;

        internal MgmtExplorerSchemaNone(CSharpType csharpType, string reason)
            : base(generateKey(csharpType), SCHEMA_TYPE)
        {
            this.Reason = reason;
        }

        public MgmtExplorerSchemaNone()
            : base()
        {

        }
    }
}
