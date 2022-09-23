// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Generation.Writers;
using AutoRest.CSharp.Mgmt.Decorator;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Models;
using Azure.ResourceManager;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal class MgmtExplorerCodeGenForResourceApi : MgmtExplorerCodeGenBase
    {
        private Resource Resource => (Resource)this.ApiDesc.Provider;

        public MgmtExplorerCodeGenForResourceApi(MgmtExplorerApiDesc apiDesc)
            : base(apiDesc)
        {
            if (!(apiDesc.Provider is Resource))
                throw new InvalidOperationException("Provider is not a Resource: " + apiDesc.Provider.Type.Name);
        }

        protected override void WriteStep_PrepareProvider(MgmtExplorerCodeGenContext context)
        {
            base.WriteStep_PrepareProvider(context);
            if (context.ArmClientVar == null)
                throw new InvalidOperationException("ArmClientVar is null");

            context.ProviderVar = MgmtExplorerCodeGenUtility.WriteGetResource(context.CodeSegmentWriter, this.Resource, context.ArmClientVar);
        }
    }
}
