// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoRest.CSharp.Input;
using AutoRest.CSharp.MgmtExplorer.Autorest;
using AutoRest.CSharp.MgmtExplorer.Models;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerExampleDesc
    {
        public MgmtExplorerCodeGenInfo? Info { get; init; }
        public string? Language { get; set; }
        public string? ServiceName { get; set; }
        public string? ResourceName { get; set; }
        public string? OperationName { get; set; }
        public string? SwaggerOperationId { get; set; }
        public string? SdkOperationId { get; set; }

        public string? ExampleName { get; set; }
        public string? OriginalFilePath { get; set; }
        public string? OriginalFileNameWithoutExtension { get; set; }

        public Dictionary<string, MgmtExplorerExampleValue> ExampleValues = new Dictionary<string, MgmtExplorerExampleValue>();

        public MgmtExplorerExampleDesc()
        {

        }

        internal MgmtExplorerExampleDesc(MgmtExplorerCodeDesc codeDesc, ExampleModel em)
        {
            this.Info = codeDesc.Info;
            this.Language = codeDesc.Language;
            this.ServiceName = codeDesc.ServiceName;
            this.ResourceName = codeDesc.ResourceName;
            this.OperationName = codeDesc.OperationName;
            this.SwaggerOperationId = codeDesc.SwaggerOperationId;
            this.SdkOperationId = codeDesc.SdkOperationId;

            this.ExampleName = em.Name;
            this.OriginalFilePath = em.OriginalFile;
            this.OriginalFileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.OriginalFilePath);

            var allMethodParameters = codeDesc.CodeSegments.SelectMany(cs => cs.Parameters);
            string[] IGNORED_PARAM_LIST =
            {
                "api-version",
            };
            foreach (var exampleParam in em.AllParameters)
            {
                var sName = exampleParam.Parameter.Language.GetSerializerNameOrName();
                if (IGNORED_PARAM_LIST.Contains(sName))
                    continue;

                var methodParameter = allMethodParameters.FirstOrDefault(p => p.SerializerName == sName);
                if (methodParameter == null)
                    throw new InvalidOperationException("unable to find parameter for example, name = " + sName);
                this.ExampleValues[sName] = new MgmtExplorerExampleValue(exampleParam.ExampleValue);
            }
        }

        public static MgmtExplorerExampleDesc FromYaml(string yaml)
        {
            return MgmtExplorerExtensions.FromYaml<MgmtExplorerExampleDesc>(yaml);
        }
    }
}
