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

        public string SuggestedUniqueOperationName => this.GetSuggestedUniqueOperatioName();

        public string ServiceName => MgmtContext.Context.DefaultLibraryName;
        public string ResourceName => this.Provider.Type.Name;
        public string OperationName => this.Operation.Name;
        public string OperationId => this.Operation.First().OperationId;
        // seems no need to include namespace to make it more readable
        public string FullApiNameWithoutNamespace => this.GetFullApiName(false /*includeNamespace*/);
        public string FullOperationNameWithoutNamespace => this.GetFullOperationName(false /*includeNamespace*/);

        public IReadOnlyList<Parameter> OperationMethodParameters => this.Operation.MethodParameters;

        public MgmtExplorerApiDesc(MgmtExplorerCodeGenInfo info, MgmtTypeProvider provider, MgmtClientOperation operation)
        {
            Info = info;
            Provider = provider;
            Operation = operation;
        }

        public string GetSuggestedUniqueOperatioName()
        {
            // TODO: does it enough to only include the first char of each type? seems should be enough in one service scope, make it configurable if we found it's not true
            string postfix = string.Join("", this.OperationMethodParameters.Select(p => p.Type.Name[0]));
            if (postfix.Length > 0)
                postfix = "_" + postfix;
            return $"{this.ServiceName}_{this.ResourceName}_{this.OperationName}{postfix}";
        }

        public string GetFullOperationName(bool includeNamespace)
        {
            return this.OperationName + "(" + string.Join(", ", this.OperationMethodParameters.Select(p => p.Type.GetFullName(includeNamespace))) + ")";
        }

        public string GetFullApiName(bool includeNamespace)
        {
            return $"{this.ServiceName}.{this.ResourceName}.{this.GetFullOperationName(includeNamespace)}";
        }
    }

}
