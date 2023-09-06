// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

#nullable disable

namespace AutoRest.SdkExplorer.Model.Azure
{
    public class AzureResourceGroup
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Region { get; set; }

        public AzureResourceGroup()
        {
        }

        public AzureResourceGroup(string name, string id, string region)
        {
            this.Name = name;
            this.Id = id;
            this.Region = region;
        }
    }
}
