// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Mgmt.Decorator;
using AutoRest.CSharp.Mgmt.Output;
using AutoRest.CSharp.MgmtExplorer.Models;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal abstract class MgmtExplorerWriterBase
    {
        protected MgmtExplorerApiDesc ApiDesc { get; private set; }

        public MgmtExplorerWriterBase(MgmtExplorerApiDesc apiDesc)
        {
            this.ApiDesc = apiDesc;
        }

        #region Write Steps
        public string WriteExplorerApi()
        {
            var context = new MgmtExplorerWriterContext();
            WriteStep_Header(context);
            WriteStep_PrepareArmClient(context);
            WriteStep_PrepareProviderHost(context);
            WriteStep_PrepareProvider(context);
            WriteStep_Invoke(context);
            WriteStep_HandleResult(context);
            WriteStep_End(context);
            return context.Writer.ToString();
        }

        protected virtual void WriteStep_Header(MgmtExplorerWriterContext context)
        {
            context.Writer.Line($"// generate {ApiDesc.UniqueName}");
            context.Writer.Line();
        }

        protected virtual void WriteStep_PrepareArmClient(MgmtExplorerWriterContext context)
        {
            context.ArmClientVar = MgmtExplorerWriterUtility.WriteGetArmClient(context.Writer);
        }

        protected virtual void WriteStep_PrepareProviderHost(MgmtExplorerWriterContext context)
        {
        }

        protected virtual void WriteStep_PrepareProvider(MgmtExplorerWriterContext context)
        {
        }

        protected virtual void WriteStep_Invoke(MgmtExplorerWriterContext context)
        {
        }

        protected virtual void WriteStep_HandleResult(MgmtExplorerWriterContext context)
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

        protected virtual void WriteStep_End(MgmtExplorerWriterContext context)
        {
            // TODO: remove this
            context.Writer.Line($"// end of generation");
        }

        #endregion
    }
}
