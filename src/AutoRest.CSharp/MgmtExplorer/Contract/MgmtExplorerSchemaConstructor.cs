// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.Output.Models.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerSchemaConstructor
    {
        public string Name { get; set; } = string.Empty;
        public MgmtExplorerSchemaConstructor? BaseConstructor { get; set; }

        public List<MgmtExplorerSchemaConstructorParameter> MethodParameters = new List<MgmtExplorerSchemaConstructorParameter>();

        public MgmtExplorerSchemaConstructor()
        {

        }

        internal MgmtExplorerSchemaConstructor(ObjectTypeConstructor ctor)
        {
            this.Name = ctor.Signature.Name;
            this.BaseConstructor = ctor.BaseConstructor == null ? null : new MgmtExplorerSchemaConstructor(ctor.BaseConstructor!);
            this.MethodParameters = ctor.Signature.Parameters.Select(p => new MgmtExplorerSchemaConstructorParameter(p, ctor.FindPropertyInitializedByParameter(p))).ToList();
        }
    }
}
