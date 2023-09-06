// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;

namespace AutoRest.SdkExplorer.Model.Azure
{
    public class AzureSubscription
    {
        public List<AzureResourceGroup> ResourceGroups { get; set; } = new List<AzureResourceGroup>();
        public List<AzureResource> Resources { get; set; } = new List<AzureResource>();
        public string SubscriptionId {get;set;}
        public string Name {get;set;}
        public string Id {get;set;}
        public bool IsDefault { get; set; }

        public AzureSubscription()
        {
        }
    }
}
