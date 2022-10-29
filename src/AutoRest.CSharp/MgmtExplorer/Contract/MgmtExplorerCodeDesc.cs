// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using AutoRest.CSharp.MgmtExplorer.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerCodeDesc
    {
        public MgmtExplorerCodeGenInfo? Info { get; init; }
        public string? Language { get; set; }
        public string? ServiceName { get; set; }
        public string? ResourceName { get; set; }
        public string? OperationName { get; set; }
        public string? SwaggerOperationId { get; set; }
        public string? SdkOperationId { get; set; }
        public List<string> ExampleNames { get; set; } = new List<string>();
        public List<MgmtExplorerCodeSegmentParameter> OperationMethodParameters { get; set; } = new List<MgmtExplorerCodeSegmentParameter>();

        public string? Description { get; set; } = string.Empty;
        public List<MgmtExplorerCodeSegment> CodeSegments { get; set; } = new List<MgmtExplorerCodeSegment>();

        public string? OperationNameWithScopeAndParameters { get; set; }
        public string? OperationNameWithParameters { get; set; }
        public string? FullUniqueName { get; set; }

        public MgmtExplorerCodeDesc()
        {
        }

        internal MgmtExplorerCodeDesc(MgmtExplorerApiDesc apiDesc)
        {
            this.Language = "DotNet";
            this.Info = apiDesc.Info;
            this.ServiceName = apiDesc.ServiceName;
            this.ResourceName = apiDesc.ResourceName;
            this.OperationName = apiDesc.OperationName;
            this.SwaggerOperationId = apiDesc.SwaggerOperationId;
            this.SdkOperationId = apiDesc.SdkOperationId;
            this.FullUniqueName = apiDesc.FullUniqueName;
            this.OperationNameWithScopeAndParameters = apiDesc.OperationNameWithScopeAndParameters;
            this.OperationNameWithParameters = apiDesc.OperationNameWithParameters;
            this.OperationMethodParameters = apiDesc.MethodParameters.Select(p => p.ToCodeSegmentParameter(false /*includeSchema*/)).ToList();
            this.ExampleNames = apiDesc.ExampleGroup?.Examples.Select(e => e.Name).ToList() ?? new List<string>();
        }

        public void AddCodeSegment(MgmtExplorerCodeSegment newSegment)
        {
            this.CodeSegments.Add(newSegment);
        }

        public string ToCode(bool applyUsing)
        {
            string newLine = "\r\n";
            var usingList = this.CodeSegments.SelectMany(s => s.UsingNamespaces)
                    .Distinct(StringComparer.Create(CultureInfo.InvariantCulture, true))
                    .OrderBy(s => s.ToLower().StartsWith("system") ? "_" + s : s).ToList();

            var usingsCode = String.Join("", usingList.Select(s => $"using {s};{newLine}"));
            var code = string.Join(newLine, this.CodeSegments.Select(s => s.Code));
            if (applyUsing)
            {
                foreach (var u in usingList.Reverse<string>())
                {
                    code = code.Replace($"global::{u}.", "");
                }
            }
            return usingsCode + newLine + code;
        }

        public string ToYaml()
        {
            var builder = new YamlDotNet.Serialization.SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var v = builder.Serialize(this);
            return v;
        }

        public static MgmtExplorerCodeDesc FromYaml(string yaml)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml
                .Build();

            return deserializer.Deserialize<MgmtExplorerCodeDesc>(yaml);
        }
    }
}
