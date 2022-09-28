// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    internal class MgmtExplorerCodeGenInfo
    {
        [JsonPropertyName("sdkPackageName")]
        public string SdkPackageName { get; set; } = string.Empty;
        [JsonPropertyName("sdkPackageVersion")]
        public string SdkPackageVersion { get; set; } = string.Empty;
        [JsonPropertyName("explorerCodeGenVersion")]
        public string ExplorerCodeGenVersion { get; set; } = "1.0.0";
        [JsonPropertyName("generatedDateTime")]
        public DateTimeOffset GeneratedDateTime { get; set; } = DateTimeOffset.Now;
        [JsonPropertyName("nugetPackages")]
        public List<string> NugetPackages { get; set; } = new List<string>();

        public MgmtExplorerCodeGenInfo()
        {
        }

        public MgmtExplorerCodeGenInfo(string sdkPackageName, string sdkPackageVersion, DateTimeOffset generatedDateTime, List<string> nugetPackages)
        {
            SdkPackageName = sdkPackageName;
            SdkPackageVersion = sdkPackageVersion;
            GeneratedDateTime = generatedDateTime;
            NugetPackages = nugetPackages.Concat(new List<string>() { $"{SdkPackageName}@{SdkPackageVersion}" }).Distinct(StringComparer.Create(CultureInfo.InvariantCulture, true)).ToList();

            this.ExplorerCodeGenVersion = "1.0.0";
        }
    }
}
