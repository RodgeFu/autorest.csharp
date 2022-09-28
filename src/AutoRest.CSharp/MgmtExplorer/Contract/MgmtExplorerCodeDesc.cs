// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using AutoRest.CSharp.MgmtExplorer.Models;
using YamlDotNet.Serialization.NamingConventions;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    internal class MgmtExplorerCodeDesc
    {
        [JsonPropertyName("info")]
        public MgmtExplorerCodeGenInfo Info { get; init; }
        [JsonPropertyName("serviceName")]
        public string ServiceName { get; set; }
        [JsonPropertyName("resourceName")]
        public string ResourceName { get; set; }
        [JsonPropertyName("operationName")]
        public string OperationName { get; set; }
        [JsonPropertyName("operationId")]
        public string OperationId { get; set; }
        [JsonPropertyName("operationMethodParameters")]
        public List<MgmtExplorerCodeSegmentParameter> OperationMethodParameters { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("codeSegments")]
        public List<MgmtExplorerCodeSegment> CodeSegments { get; } = new List<MgmtExplorerCodeSegment>();

        public MgmtExplorerCodeDesc(MgmtExplorerApiDesc apiDesc)
        {
            this.Info = apiDesc.Info;
            this.ServiceName = apiDesc.ServiceName;
            this.ResourceName = apiDesc.ResourceName;
            this.OperationName = apiDesc.OperationName;
            this.OperationId = apiDesc.OperationId;
            this.OperationMethodParameters = apiDesc.OperationMethodParameters.Select(p => new MgmtExplorerCodeSegmentParameter(p.Name, p.Name, new MgmtExplorerCodeSegmentCSharpType(p.Type), null, null)).ToList();
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
    }
}
