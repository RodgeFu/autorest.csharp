// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using AutoRest.CSharp.MgmtExplorer.Autorest;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.MgmtExplorer.Models;
using AutoRest.CSharp.Output.Builders;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal class MgmtExplorerCodeGenExampleHelper
    {
        public static void AttachExample(MgmtExplorerApiDesc apiDesc, MgmtExplorerCodeDesc codeDesc)
        {
            string[] IGNORED_PARAM_LIST =
            {
                "api-version",
            };
            if (apiDesc.ExampleGroup == null || apiDesc.ExampleGroup.Examples == null || apiDesc.ExampleGroup.Examples.Count == 0)
                return;

            var allMethodParameters = codeDesc.CodeSegments.SelectMany(cs => cs.Parameters);

            foreach (var example in apiDesc.ExampleGroup.Examples)
            {
                foreach (var exampleParam in example.AllParameters)
                {
                    var sName = exampleParam.Parameter.Language.GetSerializerNameOrName();
                    if (IGNORED_PARAM_LIST.Contains(sName))
                        continue;

                    var methodParameter = allMethodParameters.FirstOrDefault(p => p.SerializerName == sName);
                    if (methodParameter == null)
                        throw new InvalidOperationException("unable to find parameter for example, name = " + sName);
                    methodParameter.ExampleValues.Add(example.Name, new MgmtExplorerExampleValue(exampleParam.ExampleValue));
                }
            }
        }
    }
}
