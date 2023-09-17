// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

#nullable disable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoRest.SdkExplorer.Model.Azure;
using AutoRest.SdkExplorer.Model.Example;
using AutoRest.SdkExplorer.Model.Schema;
using AutoRest.SdkExplorer.Model.Hint;
using AutoRest.SdkExplorer.Utilities;

namespace AutoRest.SdkExplorer.Model.Code
{
    public class ApiDesc
    {
        public class ProviderType
        {
            public const string Resource = nameof(ProviderType.Resource);
            public const string ResourceCollection = nameof(ProviderType.ResourceCollection);
            public const string Extension = nameof(ProviderType.Extension);
        };

        public string Language { get; set; }
        public string SdkPackageName { get; set; }
        public string SdkPackageVersion { get; set; }
        public string ExplorerCodeGenVersion { get; set; }
        public string GeneratedTimestamp { get; set; }
        public List<string> Dependencies { get; set; } = new List<string>();

        public string ServiceName { get; set; }
        public string ResourceName { get; set; }
        public string OperationName { get; set; }
        public string SwaggerOperationId { get; set; }
        public string SdkOperationId { get; set; }
        public string Description { get; set; }
        public string FullUniqueName { get; set; }

        public string OperationNameWithParameters { get; set; }
        public string OperationNameWithScopeAndParameters { get; set; }

        /// <summary>
        /// Include parameter in or not in propertybag;
        /// </summary>
        public List<ParameterDesc> OperationMethodParameters { get; set; } = new List<ParameterDesc>();
        public ParameterDesc PropertyBagParameter;
        public List<CodeSegmentDesc> CodeSegments { get; set; } = new List<CodeSegmentDesc>();

        public AzureResourceType OperationProviderAzureResourceType { get; set; }
        public string OperationProviderType { get; set; }

        public string RequestPath { get; set; }
        public string ApiVersion { get; set; }

        public SchemaStore SchemaStore { get; set; } = SchemaStore.Current;

        public ApiDesc()
        {
        }


        public void AddCodeSegment(CodeSegmentDesc newSegment)
        {
            this.CodeSegments.Add(newSegment);
        }

        public string ToCode(bool applyUsing, bool useSuggestedName)
        {
            string newLine = "\r\n";
            var usingList = this.CodeSegments.SelectMany(s => s.UsingNamespaces)
                    .Distinct(StringComparer.Create(CultureInfo.InvariantCulture, true))
                    .OrderBy(s => s.ToLower().StartsWith("system") ? "_" + s : s).ToList();

            var usingsCode = String.Join("", usingList.Select(s => $"using {s};{newLine}"));
            var code = string.Join(newLine, this.CodeSegments.Select(s => useSuggestedName? s.GetCodeWithSuggestedName() : s.Code));
            if (applyUsing)
            {
                foreach (var u in usingList.Reverse<string>())
                {
                    code = code.Replace($"global::{u}.", "");
                }
            }
            return usingsCode + newLine + code;
        }

        //public void RefreshSchema(SchemaStore store)
        //{
        //    this.SchemaObjects = store.ObjectSchemas.Values.ToList();
        //    this.SchemaEnums = store.EnumSchemas.Values.ToList();
        //    this.SchemaNones = store.NoneSchemas.Values.ToList();
        //}

        public ApiHint GetApiHint(ExampleDesc ex, SchemaStore schemaStore = null)
        {
            ApiHint data = new ApiHint();
            foreach (var item in CodeSegments.SelectMany(cs => cs.GetFieldHints($"{this.ServiceName}.{this.ResourceName}.{this.OperationName}", ex, schemaStore)))
            {
                data.AppendFieldHint(item);
            }
            return data;
        }

        public string ToYaml()
        {
            return this.SerializeToYaml();
        }
    }
}
