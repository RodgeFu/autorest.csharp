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
    internal class MgmtExplorerCodeGenForResourceCollectionApi : MgmtExplorerCodeGenBase
    {
        private ResourceCollection Collection => (ResourceCollection)this.ApiDesc.Provider;

        public MgmtExplorerCodeGenForResourceCollectionApi(MgmtExplorerApiDesc apiDesc)
            : base(apiDesc)
        {
            if (!(apiDesc.Provider is ResourceCollection))
                throw new InvalidOperationException("Provider is not a ResourceCollection: " + apiDesc.Provider.Type.Name);
        }

        protected override void WriteStep_PrepareProviderHost(MgmtExplorerCodeGenContext context)
        {
            base.WriteStep_PrepareProviderHost(context);

            if (context.ArmClientVar == null)
                throw new InvalidOperationException("ArmClientVar is null");

            var hostList = this.Collection.Parent();
            // when will this not be 1? let's see
            Debug.Assert(hostList?.Count() == 1);
            var host = hostList.First();

            if (host is MgmtExtensions)
            {
                var ext = (MgmtExtensions)host;
                var hostName = ext.ArmCoreType.Name;
                if (ext.ArmCoreType == typeof(ArmResource))
                {
                    // seems we need to do nothing here, when will this occur?
                    Debug.Assert(false);
                    throw new NotImplementedException();
                }
                else
                {
                    context.ProviderHostVar = MgmtExplorerCodeGenUtility.WriteGetExtensionResource(context.Writer, ext, context.ArmClientVar);
                }

            }
            else if (host is Resource)
            {
                var hostName = host.Type.Name;
                context.ProviderHostVar = MgmtExplorerCodeGenUtility.WriteGetResource(context.Writer, (Resource)host, context.ArmClientVar);
            }
            else
            {
                throw new InvalidOperationException("unexpected type of ResourceCollection's host: " + host.Type.Name);
            }
        }

        protected override void WriteStep_PrepareProvider(MgmtExplorerCodeGenContext context)
        {
            base.WriteStep_PrepareProvider(context);

            if (context.ProviderHostVar == null)
            {
                // TODO: WHEN WILL THIS HAPPEN?
                Debugger.Break();
                throw new NotImplementedException();
            }
            else
            {
                context.ProviderVar = MgmtExplorerCodeGenUtility.WriteGetResourceCollection(context.Writer, this.Collection, context.ProviderHostVar);
            }
        }
    }
}
