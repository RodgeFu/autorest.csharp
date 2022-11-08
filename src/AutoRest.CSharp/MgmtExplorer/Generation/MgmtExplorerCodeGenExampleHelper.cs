// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.MgmtExplorer.Autorest;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.MgmtExplorer.Models;
using AutoRest.CSharp.Output.Builders;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal class MgmtExplorerCodeGenExampleHelper
    {
        public static List<MgmtExplorerExampleDesc> GenerateExampleDescs(MgmtExplorerApiDesc apiDesc, MgmtExplorerCodeDesc codeDesc)
        {
            if (apiDesc.ExampleGroup == null || apiDesc.ExampleGroup.Examples == null || apiDesc.ExampleGroup.Examples.Count == 0)
                return new List<MgmtExplorerExampleDesc>();

            return apiDesc.ExampleGroup.Examples.Select(e => new MgmtExplorerExampleDesc(codeDesc, e)).ToList();
        }
    }
}
