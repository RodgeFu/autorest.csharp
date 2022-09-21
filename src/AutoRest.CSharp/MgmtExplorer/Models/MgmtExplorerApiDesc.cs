// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using AutoRest.CSharp.Mgmt.Models;
using AutoRest.CSharp.Mgmt.Output;

namespace AutoRest.CSharp.MgmtExplorer.Models
{
    internal class MgmtExplorerApiDesc
    {
        public MgmtExplorerApiDesc(MgmtTypeProvider provider, MgmtClientOperation operation)
        {
            Provider = provider;
            Operation = operation;
        }

        public MgmtTypeProvider Provider { get; }
        public MgmtClientOperation Operation { get; }

        public string UniqueName => $"{Provider.Type.Name}__{Operation.First().OperationId}__{Operation.Name}";
        public string Description => $"Method {Operation.Name}() on {Provider.ResourceName}";
    }

}
