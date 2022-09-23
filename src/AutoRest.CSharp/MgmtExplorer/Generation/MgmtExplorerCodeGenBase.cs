﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Mgmt.Decorator;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.MgmtExplorer.Models;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal abstract class MgmtExplorerCodeGenBase
    {
        protected string LocalId = Guid.NewGuid().ToString();
        protected MgmtExplorerApiDesc ApiDesc { get; private set; }

        public MgmtExplorerCodeGenBase(MgmtExplorerApiDesc apiDesc)
        {
            this.ApiDesc = apiDesc;
        }

        #region Write Steps
        public string WriteExplorerApi()
        {
            var context = new MgmtExplorerCodeGenContext("GET_ARM_CLIENT", "GetArmClient");
            // Now we separate the code into two segment: Get_Client and Invoke_Api
            // Consider separate it into more segment like Get_Tenant, Get_Subscription considering it may be shared between multiple APIs
            // *if it worth the increasement of complexity*
            WriteStep_Header(context);

            WriteStep_PrepareArmClient(context);
            if (context.ArmClientVar == null)
                throw new InvalidOperationException("context.ArmClientVar is null");
            var seg = new MgmtExplorerCodeSegment("GET_ARM_CLIENT", "GetArmClient");
            context.PushCodeSegment((oldSegment) =>
            {
                oldSegment.OutputResult = new List<MgmtExplorerCodeSegmentVariable>() { context.ArmClientVar.AsCodeSegmentVariable() };
                oldSegment.Scope = MgmtExplorerCodeSegment.CodeSegmentScope.Global;
            }, "INVOKE_API_" + LocalId, "Invoke_" + this.ApiDesc.UniqueName);

            WriteStep_PrepareProviderHost(context);
            WriteStep_PrepareProvider(context);
            WriteStep_Invoke(context);
            WriteStep_HandleResult(context);
            WriteStep_End(context);
            context.PushCodeSegment((oldSegment) =>
            {
                oldSegment.Dependencies = new List<MgmtExplorerCodeSegmentVariable>() { context.ArmClientVar.AsCodeSegmentVariable() };
                oldSegment.OutputResult = context.ResultVar == null ? new List<MgmtExplorerCodeSegmentVariable>() : new List<MgmtExplorerCodeSegmentVariable>() { context.ResultVar.AsCodeSegmentVariable() };
                oldSegment.Scope = MgmtExplorerCodeSegment.CodeSegmentScope.Local;
            });

            return context.ExplorerCode.ToString();
        }

        protected virtual void WriteStep_Header(MgmtExplorerCodeGenContext context)
        {
            string desc = $"generate {ApiDesc.UniqueName}";
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
            if (op.IsLongRunningOperation)
            {
                context.ResultVar = MgmtExplorerCodeGenUtility.WriteInvokeLongRunningOperation(context.CodeSegmentWriter, op, context.ProviderVar);
            }
            else if (op.IsPagingOperation)
            {
                context.ResultVar = MgmtExplorerCodeGenUtility.WriteInvokePagedOperation(context.CodeSegmentWriter, op, context.ProviderVar);
            }
            else
            {
                context.ResultVar = MgmtExplorerCodeGenUtility.WriteInvokeNormalOperation(context.CodeSegmentWriter, op, context.ProviderVar);
            }

        }

        protected virtual void WriteStep_HandleResult(MgmtExplorerCodeGenContext context)
        {
            if (context.ResultVar != null)
            {
                CSharpType r = context.ResultVar.Type;
                context.CodeSegmentWriter.Line($"Console.WriteLine(\"Result returned for {this.ApiDesc.UniqueName}:\")");
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
                context.CodeSegmentWriter.Line($"Console.WriteLine(\"No result returned for {this.ApiDesc.UniqueName}\")");
            }
        }

        protected virtual void WriteStep_End(MgmtExplorerCodeGenContext context)
        {
        }

        #endregion
    }
}
