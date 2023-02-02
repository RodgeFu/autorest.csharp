// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.Common.Output.Models.Types;
using AutoRest.CSharp.Output.Models;
using AutoRest.CSharp.Output.Models.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerSchemaMethod
    {
        public string Name { get; set; } = string.Empty;
        public List<MgmtExplorerSchemaMethodParameter> MethodParameters = new List<MgmtExplorerSchemaMethodParameter>();

        public MgmtExplorerSchemaMethod()
        {

        }

        internal MgmtExplorerSchemaMethod(ObjectTypeConstructor ctor)
        {
            this.Name = ctor.Signature.Name;
            this.MethodParameters = ctor.Signature.Parameters.Select(p => new MgmtExplorerSchemaMethodParameter(p, ctor.FindPropertyInitializedByParameter(p))).ToList();
        }
    }
}
