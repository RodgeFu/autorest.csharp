// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Azure.Core;

namespace AutoRest.SdkExplorer.Model.Azure
{
    public class AzureResourceType : IEquatable<AzureResourceType>
    {
        public string Namespace { get; set; }
        public string Type { get; set; }

        public AzureResourceType() { }

        [JsonConstructor]
        public AzureResourceType(string @namespace, string type)
        {
            Namespace = @namespace;
            Type = type;
        }

        /// <summary>
        /// sth like Microsoft.Storage/storageAccounts/tableServices/tables
        /// </summary>
        /// <param name="typeString"></param>
        public AzureResourceType(string typeString)
        {
            int index = typeString.IndexOf('/');
            if (index < 0)
                throw new InvalidOperationException("invalid typeString: " + typeString);
            Namespace = typeString.Substring(0, index);
            Type = typeString.Substring(index + 1);
        }

        internal AzureResourceType(ResourceType resourceType)
            : this(resourceType.Namespace, resourceType.Type)
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AzureResourceType);
        }

        public bool Equals(AzureResourceType other)
        {
            return other is not null &&
                   Namespace == other.Namespace &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Namespace, Type);
        }

        public static bool operator ==(AzureResourceType left, AzureResourceType right)
        {
            return EqualityComparer<AzureResourceType>.Default.Equals(left, right);
        }

        public static bool operator !=(AzureResourceType left, AzureResourceType right)
        {
            return !(left == right);
        }
    }
}
