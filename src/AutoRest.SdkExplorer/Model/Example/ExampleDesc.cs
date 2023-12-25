// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using System;
using System.Collections.Generic;
using AutoRest.SdkExplorer.Utilities;

namespace AutoRest.SdkExplorer.Model.Example
{
    public class ExampleDesc
    {
        public string? Language { get; set; }
        public string? SdkPackageName { get; set; }
        public string? SdkPackageVersion { get; set; }
        public string? ExplorerCodeGenVersion { get; set; }
        public string? GeneratedTimestamp { get; set; }

        public string? ServiceName { get; set; }
        public string? ResourceName { get; set; }
        public string? OperationName { get; set; }
        public string? SwaggerOperationId { get; set; }
        public string? SdkOperationId { get; set; }
        public string? SdkFullUniqueName { get; set; }

        public string? ExampleName { get; set; }
        public string? OriginalFilePath { get; set; }
        public string? OriginalFileNameWithoutExtension { get; set; }

        public string? AiDescription { get; set; }

        public string? EmbeddingText { get; set; }
        public string? EmbeddingVector { get; set; }

        public Dictionary<string, ExampleValueDesc> ExampleValues { get; set; } = new Dictionary<string, ExampleValueDesc>();

        public ExampleDesc()
        {

        }

        public string ToYaml()
        {
            return this.SerializeToYaml();
        }
    }
}
