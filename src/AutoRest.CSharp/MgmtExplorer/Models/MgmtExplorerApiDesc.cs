// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.Mgmt.AutoRest;
using AutoRest.CSharp.Mgmt.Models;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Autorest;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.Output.Models.Shared;

namespace AutoRest.CSharp.MgmtExplorer.Models
{
    internal class MgmtExplorerApiDesc
    {
        public MgmtTypeProvider Provider { get; }
        public MgmtClientOperation Operation { get; }

        public string Description => $"Method {Operation.Name}() on {Provider.Type.Name}";

        public MgmtExplorerCodeGenInfo Info { get; set; }

        public string FullUniqueName => $"{ServiceName}_{ResourceName}_{SdkOperationId}";

        public string ServiceName => MgmtContext.Context.DefaultLibraryName;
        public string ResourceName => this.Provider.Type.Name;
        public string OperationName => this.Operation.Name;
        public string SwaggerOperationId => this.Operation.First().OperationId;
        public string SdkOperationId => $"{this.OperationName}_For_OperationId_{this.SwaggerOperationId}";
        // seems no need to include namespace to make it more readable
        public string OperationNameWithParameters => this.GetOperationNameWithParameters(false /*includeNamespace*/);
        public string OperationNameWithScopeAndParameters => this.GetOperationNameWithScopeAndParameters(false /*includeNamespace*/);

        public IReadOnlyList<Parameter> OperationMethodParameters => this.Operation.MethodParameters;

        public MgmtExplorerApiDesc(MgmtExplorerCodeGenInfo info, MgmtTypeProvider provider, MgmtClientOperation operation)
        {
            Info = info;
            Provider = provider;
            Operation = operation;
        }

        public string GetOperationNameWithParameters(bool includeNamespace)
        {
            return this.OperationName + "(" + string.Join(", ", this.OperationMethodParameters.Select(p => p.Type.GetFullName(includeNamespace))) + ")";
        }

        public string GetOperationNameWithScopeAndParameters(bool includeNamespace)
        {
            return $"{this.ServiceName}.{this.ResourceName}.{this.GetOperationNameWithParameters(includeNamespace)}";
        }
    }

}
