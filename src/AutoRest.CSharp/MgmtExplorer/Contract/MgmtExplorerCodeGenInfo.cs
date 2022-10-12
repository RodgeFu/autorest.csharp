// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerCodeGenInfo
    {
        public string? SdkPackageName { get; set; }
        public string? SdkPackageVersion { get; set; }
        public string ExplorerCodeGenVersion { get; set; } = "1.0.0";
        public string? GeneratedTimestamp { get; set; }
        public List<string> Dependencies { get; set; } = new List<string>();

        public MgmtExplorerCodeGenInfo()
        {
        }

        public MgmtExplorerCodeGenInfo(string sdkPackageName, string sdkPackageVersion, DateTimeOffset generatedTimestamp, List<string> nugetPackages)
        {
            SdkPackageName = sdkPackageName;
            SdkPackageVersion = sdkPackageVersion;
            GeneratedTimestamp = generatedTimestamp.ToString("yyyy-MM-dd_HH-mm-ss-ffffff");
            Dependencies = nugetPackages.Concat(new List<string>() { $"{SdkPackageName}@{SdkPackageVersion}" }).Distinct(StringComparer.Create(CultureInfo.InvariantCulture, true)).ToList();

            this.ExplorerCodeGenVersion = "1.0.0";
        }
    }
}
