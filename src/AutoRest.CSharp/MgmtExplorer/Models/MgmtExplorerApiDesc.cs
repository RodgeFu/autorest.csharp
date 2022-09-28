// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.Mgmt.AutoRest;
using AutoRest.CSharp.Mgmt.Models;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.Output.Models.Shared;

namespace AutoRest.CSharp.MgmtExplorer.Models
{
    internal class MgmtExplorerApiDesc
    {
        public MgmtTypeProvider Provider { get; }
        public MgmtClientOperation Operation { get; }

        public string UniqueName => $"{Provider.Type.Name}_{Operation.Name}_For_OperationId_{Operation.First().OperationId}";
        public string Description => $"Method {Operation.Name}() on {Provider.Type.Name}";

        public MgmtExplorerCodeGenInfo Info { get; set; }

        public string ServiceName => MgmtContext.Context.DefaultNamespace;
        public string ResourceName => this.Provider.Type.Name;
        public string OperationName => this.Operation.Name;
        public string OperationId => this.Operation.First().OperationId;

        public IReadOnlyList<Parameter> OperationMethodParameters => this.Operation.MethodParameters;

        public MgmtExplorerApiDesc(MgmtExplorerCodeGenInfo info, MgmtTypeProvider provider, MgmtClientOperation operation)
        {
            Info = info;
            Provider = provider;
            Operation = operation;
        }

    }

}
