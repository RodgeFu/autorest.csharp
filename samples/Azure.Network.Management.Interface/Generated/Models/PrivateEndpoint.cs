// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace Azure.Network.Management.Interface.Models
{
    /// <summary> Private endpoint resource. </summary>
    public partial class PrivateEndpoint : Resource
    {
        /// <summary> Initializes a new instance of <see cref="PrivateEndpoint"/>. </summary>
        public PrivateEndpoint()
        {
            NetworkInterfaces = new ChangeTrackingList<NetworkInterface>();
            PrivateLinkServiceConnections = new ChangeTrackingList<PrivateLinkServiceConnection>();
            ManualPrivateLinkServiceConnections = new ChangeTrackingList<PrivateLinkServiceConnection>();
        }

        /// <summary> Initializes a new instance of <see cref="PrivateEndpoint"/>. </summary>
        /// <param name="id"> Resource ID. </param>
        /// <param name="name"> Resource name. </param>
        /// <param name="type"> Resource type. </param>
        /// <param name="location"> Resource location. </param>
        /// <param name="tags"> Resource tags. </param>
        /// <param name="etag"> A unique read-only string that changes whenever the resource is updated. </param>
        /// <param name="subnet"> The ID of the subnet from which the private IP will be allocated. </param>
        /// <param name="networkInterfaces"> An array of references to the network interfaces created for this private endpoint. </param>
        /// <param name="provisioningState"> The provisioning state of the private endpoint resource. </param>
        /// <param name="privateLinkServiceConnections"> A grouping of information about the connection to the remote resource. </param>
        /// <param name="manualPrivateLinkServiceConnections"> A grouping of information about the connection to the remote resource. Used when the network admin does not have access to approve connections to the remote resource. </param>
        internal PrivateEndpoint(string id, string name, string type, string location, IDictionary<string, string> tags, string etag, Subnet subnet, IReadOnlyList<NetworkInterface> networkInterfaces, ProvisioningState? provisioningState, IList<PrivateLinkServiceConnection> privateLinkServiceConnections, IList<PrivateLinkServiceConnection> manualPrivateLinkServiceConnections) : base(id, name, type, location, tags)
        {
            Etag = etag;
            Subnet = subnet;
            NetworkInterfaces = networkInterfaces;
            ProvisioningState = provisioningState;
            PrivateLinkServiceConnections = privateLinkServiceConnections;
            ManualPrivateLinkServiceConnections = manualPrivateLinkServiceConnections;
        }

        /// <summary> A unique read-only string that changes whenever the resource is updated. </summary>
        public string Etag { get; }
        /// <summary> The ID of the subnet from which the private IP will be allocated. </summary>
        public Subnet Subnet { get; set; }
        /// <summary> An array of references to the network interfaces created for this private endpoint. </summary>
        public IReadOnlyList<NetworkInterface> NetworkInterfaces { get; }
        /// <summary> The provisioning state of the private endpoint resource. </summary>
        public ProvisioningState? ProvisioningState { get; }
        /// <summary> A grouping of information about the connection to the remote resource. </summary>
        public IList<PrivateLinkServiceConnection> PrivateLinkServiceConnections { get; }
        /// <summary> A grouping of information about the connection to the remote resource. Used when the network admin does not have access to approve connections to the remote resource. </summary>
        public IList<PrivateLinkServiceConnection> ManualPrivateLinkServiceConnections { get; }
    }
}
