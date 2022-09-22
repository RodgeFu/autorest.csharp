// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Mgmt.Decorator;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Models;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal abstract class MgmtExplorerCodeGenBase
    {
        protected MgmtExplorerApiDesc ApiDesc { get; private set; }

        public MgmtExplorerCodeGenBase(MgmtExplorerApiDesc apiDesc)
        {
            this.ApiDesc = apiDesc;
        }

        #region Write Steps
        public string WriteExplorerApi()
        {
            var context = new MgmtExplorerCodeGenContext();
            WriteStep_Header(context);
            WriteStep_PrepareArmClient(context);
            WriteStep_PrepareProviderHost(context);
            WriteStep_PrepareProvider(context);
            WriteStep_Invoke(context);
            WriteStep_HandleResult(context);
            WriteStep_End(context);
            return context.Writer.ToString();
        }

        protected virtual void WriteStep_Header(MgmtExplorerCodeGenContext context)
        {
            string desc = $"generate {ApiDesc.UniqueName}";
            context.Writer.Line($"// {desc}");
            context.Writer.Line();
            context.ExplorerCodeWriter.Description = desc;
        }

        protected virtual void WriteStep_PrepareArmClient(MgmtExplorerCodeGenContext context)
        {
            context.ArmClientVar = MgmtExplorerCodeGenUtility.WriteGetArmClient(context.Writer);
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
                throw new InvalidOperationException("ProviderVar is null when calling WriteStep_Invoke");

            var op = this.ApiDesc.Operation;
            if (op.IsLongRunningOperation)
            {
                context.ResultVar = MgmtExplorerCodeGenUtility.WriteInvokeLongRunningOperation(context.Writer, op, context.ProviderVar);
            }
            else if (op.IsPagingOperation)
            {
                context.ResultVar = MgmtExplorerCodeGenUtility.WriteInvokePagedOperation(context.Writer, op, context.ProviderVar);
            }
            else
            {
                context.ResultVar = MgmtExplorerCodeGenUtility.WriteInvokeNormalOperation(context.Writer, op, context.ProviderVar);
            }
        }

        protected virtual void WriteStep_HandleResult(MgmtExplorerCodeGenContext context)
        {
            if (context.ResultVar != null)
            {
                CSharpType r = context.ResultVar.Type;
                if (r.IsFrameworkType && r.FrameworkType == typeof(List<>))
                {
                    context.Writer.Line($"Console.WriteLine(\"{context.ResultVar.Declaration}.Count = \" + {context.ResultVar.Declaration}.Count)");
                }
                else if (!r.IsFrameworkType && r.Implementation is Resource)
                {
                    context.Writer.Line($"Console.WriteLine(\"{context.ResultVar.Declaration}.Data.Id = \" + {context.ResultVar.Declaration}.Data.Id)");
                }
                else if (!r.IsFrameworkType && r.IsResourceDataType(out ResourceData? data))
                {
                    context.Writer.Line($"Console.WriteLine(\"{context.ResultVar.Declaration}.Id = \" + {context.ResultVar.Declaration}.Id)");
                }
                else
                {
                    context.Writer.Line($"Console.WriteLine(\"{context.ResultVar.Declaration}.ToString() = \" + {context.ResultVar.Declaration}.ToString())");
                }
            }
            else
            {
                context.Writer.Line($"Console.WriteLine(\"No result returned\")");
            }
        }

        protected virtual void WriteStep_End(MgmtExplorerCodeGenContext context)
        {
            // TODO: remove this
            context.Writer.Line($"// end of generation");
        }

        #endregion
    }
}
