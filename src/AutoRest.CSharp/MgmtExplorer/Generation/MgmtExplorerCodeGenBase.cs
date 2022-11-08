﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Mgmt.Decorator;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.MgmtExplorer.Models;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal abstract class MgmtExplorerCodeGenBase
    {
        public static MgmtExplorerCodeGenBase Create(MgmtExplorerApiDesc apiDesc)
        {
            return (apiDesc.Provider) switch
            {
                ResourceCollection rc => new MgmtExplorerCodeGenForResourceCollectionApi(apiDesc),
                Resource res => new MgmtExplorerCodeGenForResourceApi(apiDesc),
                MgmtExtensions ex => new MgmtExplorerCodeGenForExtensionsApi(apiDesc),
                // TODO: throw exception after we add all support
                _ => throw new InvalidOperationException("Unexpected apiDesc.Provider type: " + apiDesc.Provider.GetType().ToString()),
            };
        }

        protected string LocalId = Guid.NewGuid().ToString();
        protected MgmtExplorerApiDesc ApiDesc { get; private set; }

        public MgmtExplorerCodeGenBase(MgmtExplorerApiDesc apiDesc)
        {
            this.ApiDesc = apiDesc;
        }

        #region Write Steps
        public MgmtExplorerCodeDesc WriteExplorerApi()
        {
            MgmtExplorerCodeGenSchemaStore.CreateStore();

            var context = new MgmtExplorerCodeGenContext(this.ApiDesc, "GET_ARM_CLIENT", "GetArmClient");

            // Now we separate the code into two segment: Get_Client and Invoke_Api
            // Consider separate it into more segment like Get_Tenant, Get_Subscription considering it may be shared between multiple APIs
            // *if it worth the increasement of complexity*
            WriteStep_Header(context);

            WriteStep_PrepareArmClient(context);
            if (context.ArmClientVar == null)
                throw new InvalidOperationException("context.ArmClientVar is null");
            context.PushCodeSegment((oldSegment) =>
            {
                oldSegment.OutputResult = new List<MgmtExplorerCodeSegmentVariable>() { context.ArmClientVar.AsCodeSegmentVariable() };
            }, "INVOKE_API_" + LocalId, "Invoke_" + this.ApiDesc.FullUniqueName );

            WriteStep_PrepareProviderHost(context);
            WriteStep_PrepareProvider(context);
            WriteStep_Invoke(context);
            WriteStep_HandleResult(context);
            WriteStep_End(context);
            context.PushCodeSegment((oldSegment) =>
            {
                oldSegment.Dependencies = new List<MgmtExplorerCodeSegmentVariable>() { context.ArmClientVar.AsCodeSegmentVariable() };
                oldSegment.OutputResult = context.ResultVar == null ? new List<MgmtExplorerCodeSegmentVariable>() : new List<MgmtExplorerCodeSegmentVariable>() { context.ResultVar.AsCodeSegmentVariable() };
            });

            context.ExplorerCode.RefreshSchema();
            return context.ExplorerCode;
        }

        protected virtual void WriteStep_Header(MgmtExplorerCodeGenContext context)
        {
            string desc = $"Code generated for {ApiDesc.OperationNameWithScopeAndParameters}";
            context.ExplorerCode.Description = desc;
        }

        protected virtual void WriteStep_PrepareArmClient(MgmtExplorerCodeGenContext context)
        {
            context.ArmClientVar = MgmtExplorerCodeGenUtility.WriteGetArmClient(context.CodeSegmentWriter);
        }

        protected virtual void WriteStep_PrepareProviderHost(MgmtExplorerCodeGenContext context)
        {
        }

        protected virtual void WriteStep_PrepareProvider(MgmtExplorerCodeGenContext context)
        {
        }

        protected virtual void WriteStep_Invoke(MgmtExplorerCodeGenContext context)
        {
            if (context.ProviderVar == null)
                throw new ArgumentNullException("context.ProviderVar");
            if (context.ArmClientVar == null)
                throw new ArgumentNullException("context.ArmClientVar");

            var op = this.ApiDesc.Operation;
            var parameters = this.ApiDesc.MethodParameters.ToList();
            if (op.IsLongRunningOperation)
            {
                context.ResultVar = MgmtExplorerCodeGenUtility.WriteInvokeLongRunningOperation(context.CodeSegmentWriter, op, parameters, context.ProviderVar);
            }
            else if (op.IsPagingOperation)
            {
                context.ResultVar = MgmtExplorerCodeGenUtility.WriteInvokePagedOperation(context.CodeSegmentWriter, op, parameters, context.ProviderVar);
            }
            else
            {
                context.ResultVar = MgmtExplorerCodeGenUtility.WriteInvokeNormalOperation(context.CodeSegmentWriter, op, parameters, context.ProviderVar);
            }

        }

        protected virtual void WriteStep_HandleResult(MgmtExplorerCodeGenContext context)
        {
            if (context.ResultVar != null)
            {
                CSharpType r = context.ResultVar.Type;
                context.CodeSegmentWriter.Line($"Console.WriteLine(\"Result returned for {this.ApiDesc.OperationNameWithScopeAndParameters}:\")");
                if (r.IsFrameworkType && r.FrameworkType == typeof(List<>))
                {
                    context.CodeSegmentWriter.Line($"Console.WriteLine(\"  {context.ResultVar.KeyDeclaration}.Count = \" + {context.ResultVar.KeyDeclaration}.Count)");
                }
                else if (!r.IsFrameworkType && r.Implementation is Resource)
                {
                    context.CodeSegmentWriter.Line($"Console.WriteLine(\"  {context.ResultVar.KeyDeclaration}.Data.Id = \" + {context.ResultVar.KeyDeclaration}.Data.Id)");
                }
                else if (!r.IsFrameworkType && r.IsResourceDataType(out ResourceData? data))
                {
                    context.CodeSegmentWriter.Line($"Console.WriteLine(\"  {context.ResultVar.KeyDeclaration}.Id = \" + {context.ResultVar.KeyDeclaration}.Id)");
                }
                else
                {
                    context.CodeSegmentWriter.Line($"Console.WriteLine(\"  {context.ResultVar.KeyDeclaration}.ToString() = \" + {context.ResultVar.KeyDeclaration}.ToString())");
                }
            }
            else
            {
                context.CodeSegmentWriter.Line($"Console.WriteLine(\"No result returned for {this.ApiDesc.OperationNameWithScopeAndParameters}\")");
            }
        }

        protected virtual void WriteStep_End(MgmtExplorerCodeGenContext context)
        {
        }

        #endregion
    }
}
