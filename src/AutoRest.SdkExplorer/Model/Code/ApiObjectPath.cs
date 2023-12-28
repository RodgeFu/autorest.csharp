// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoRest.SdkExplorer.Model.Example;

namespace AutoRest.SdkExplorer.Model.Code
{
    public class ApiObjectPath : IEquatable<ApiObjectPath?>
    {
        private const string SEP = "/";

        public string ServiceName { get; set; }
        public string? ResourceName { get; set; }
        public string? OperationFamilyName { get; set; }
        public string? Version { get; set; }

        [JsonConstructor]
        public ApiObjectPath(string serviceName, string? resourceName, string? operationFamilyName, string? version)
        {
            ServiceName = serviceName;
            ResourceName = resourceName;
            OperationFamilyName = operationFamilyName;
            Version = version;
        }

        public ApiObjectPath(ApiDesc desc)
        {
            ServiceName = desc.ServiceName;
            ResourceName = desc.ResourceName;
            OperationFamilyName = desc.OperationName;
            Version = desc.SdkPackageVersion;
        }

        public ApiObjectPath(ExampleDesc desc)
        {
            ServiceName = desc.ServiceName ?? "";
            ResourceName = desc.ResourceName;
            OperationFamilyName = desc.OperationName;
            Version = desc.SdkPackageVersion;
        }

        public string AsPathString()
        {
            return $"{this.ServiceName}{SEP}{this.ResourceName ?? ""}{SEP}{this.OperationFamilyName ?? ""}{SEP}{this.Version ?? ""}";
        }

        public override string ToString()
        {
            return this.AsPathString();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ApiObjectPath);
        }

        public bool Equals(ApiObjectPath? other)
        {
            return other is not null &&
                   ServiceName == other.ServiceName &&
                   ResourceName == other.ResourceName &&
                   OperationFamilyName == other.OperationFamilyName &&
                   Version == other.Version;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ServiceName, ResourceName, OperationFamilyName, Version);
        }

        public static bool operator ==(ApiObjectPath? left, ApiObjectPath? right)
        {
            return EqualityComparer<ApiObjectPath>.Default.Equals(left, right);
        }

        public static bool operator !=(ApiObjectPath? left, ApiObjectPath? right)
        {
            return !(left == right);
        }
    }
}
