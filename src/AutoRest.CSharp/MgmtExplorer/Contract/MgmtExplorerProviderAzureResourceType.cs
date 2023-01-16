// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

public class MgmtExplorerProviderAzureResourceType
{
    public string? Namespace { get;set;}
    public string? Type { get; set; }

    public MgmtExplorerProviderAzureResourceType()
    {

    }

    public MgmtExplorerProviderAzureResourceType(string @namespace, string type)
    {
        this.Namespace = @namespace;
        this.Type = type;
    }
};
