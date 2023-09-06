// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

#nullable disable

namespace AutoRest.SdkExplorer.Model.Azure
{
    public class AzureResource
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Region { get; set; }
        public AzureResourceType Type { get; set; }
        public string ResourceGroupName { get; set; }
        public string SubscriptionId { get; set; }

        public AzureResource() { }

        public AzureResource(string id, string region)
        {
            AzureResourceIdentifier ari = new AzureResourceIdentifier(id);
            this.Name = ari.GetResourceGroupName();
            this.Id = ari.GetId();
            this.Region = region;
            this.Type = ari.GetResourceType();
            this.ResourceGroupName = ari.GetResourceGroupName();
            this.SubscriptionId = ari.GetSubscriptionId();
        }
    }
}
