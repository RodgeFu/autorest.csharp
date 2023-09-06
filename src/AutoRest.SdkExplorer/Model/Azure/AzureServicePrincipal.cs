// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

#nullable disable

namespace AutoRest.SdkExplorer.Model.Azure
{
    public class AzureServicePrincipal
    {
        public string Name {get;set;}
        public string AppId  {get;set;}
        public string TenantId { get; set; }

        public AzureServicePrincipal(string name, string appId, string tenantId)
        {
            this.Name = name;
            this.AppId = appId;
            this.TenantId = tenantId;
        }
    }
}
