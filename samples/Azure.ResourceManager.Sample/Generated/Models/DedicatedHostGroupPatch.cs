// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;
using Azure.ResourceManager.Resources.Models;

namespace Azure.ResourceManager.Sample.Models
{
    /// <summary>
    /// Specifies information about the dedicated host group that the dedicated host should be assigned to. Only tags may be updated.
    /// Serialized Name: DedicatedHostGroupUpdate
    /// </summary>
    public partial class DedicatedHostGroupPatch : UpdateResource
    {
        /// <summary> Initializes a new instance of <see cref="DedicatedHostGroupPatch"/>. </summary>
        public DedicatedHostGroupPatch()
        {
            Zones = new ChangeTrackingList<string>();
            Hosts = new ChangeTrackingList<Resources.Models.SubResource>();
        }

        /// <summary> Initializes a new instance of <see cref="DedicatedHostGroupPatch"/>. </summary>
        /// <param name="tags">
        /// Resource tags
        /// Serialized Name: UpdateResource.tags
        /// </param>
        /// <param name="zones">
        /// Availability Zone to use for this host group. Only single zone is supported. The zone can be assigned only during creation. If not provided, the group supports all zones in the region. If provided, enforces each host in the group to be in the same zone.
        /// Serialized Name: DedicatedHostGroupUpdate.zones
        /// </param>
        /// <param name="platformFaultDomainCount">
        /// Number of fault domains that the host group can span.
        /// Serialized Name: DedicatedHostGroupUpdate.properties.platformFaultDomainCount
        /// </param>
        /// <param name="hosts">
        /// A list of references to all dedicated hosts in the dedicated host group.
        /// Serialized Name: DedicatedHostGroupUpdate.properties.hosts
        /// </param>
        /// <param name="instanceView">
        /// The dedicated host group instance view, which has the list of instance view of the dedicated hosts under the dedicated host group.
        /// Serialized Name: DedicatedHostGroupUpdate.properties.instanceView
        /// </param>
        /// <param name="supportAutomaticPlacement">
        /// Specifies whether virtual machines or virtual machine scale sets can be placed automatically on the dedicated host group. Automatic placement means resources are allocated on dedicated hosts, that are chosen by Azure, under the dedicated host group. The value is defaulted to 'true' when not provided. &lt;br&gt;&lt;br&gt;Minimum api-version: 2020-06-01.
        /// Serialized Name: DedicatedHostGroupUpdate.properties.supportAutomaticPlacement
        /// </param>
        internal DedicatedHostGroupPatch(IDictionary<string, string> tags, IList<string> zones, int? platformFaultDomainCount, IReadOnlyList<Resources.Models.SubResource> hosts, DedicatedHostGroupInstanceView instanceView, bool? supportAutomaticPlacement) : base(tags)
        {
            Zones = zones;
            PlatformFaultDomainCount = platformFaultDomainCount;
            Hosts = hosts;
            InstanceView = instanceView;
            SupportAutomaticPlacement = supportAutomaticPlacement;
        }

        /// <summary>
        /// Availability Zone to use for this host group. Only single zone is supported. The zone can be assigned only during creation. If not provided, the group supports all zones in the region. If provided, enforces each host in the group to be in the same zone.
        /// Serialized Name: DedicatedHostGroupUpdate.zones
        /// </summary>
        public IList<string> Zones { get; }
        /// <summary>
        /// Number of fault domains that the host group can span.
        /// Serialized Name: DedicatedHostGroupUpdate.properties.platformFaultDomainCount
        /// </summary>
        public int? PlatformFaultDomainCount { get; set; }
        /// <summary>
        /// A list of references to all dedicated hosts in the dedicated host group.
        /// Serialized Name: DedicatedHostGroupUpdate.properties.hosts
        /// </summary>
        public IReadOnlyList<Resources.Models.SubResource> Hosts { get; }
        /// <summary>
        /// The dedicated host group instance view, which has the list of instance view of the dedicated hosts under the dedicated host group.
        /// Serialized Name: DedicatedHostGroupUpdate.properties.instanceView
        /// </summary>
        internal DedicatedHostGroupInstanceView InstanceView { get; }
        /// <summary>
        /// List of instance view of the dedicated hosts under the dedicated host group.
        /// Serialized Name: DedicatedHostGroupInstanceView.hosts
        /// </summary>
        public IReadOnlyList<DedicatedHostInstanceViewWithName> InstanceViewHosts
        {
            get => InstanceView?.Hosts;
        }

        /// <summary>
        /// Specifies whether virtual machines or virtual machine scale sets can be placed automatically on the dedicated host group. Automatic placement means resources are allocated on dedicated hosts, that are chosen by Azure, under the dedicated host group. The value is defaulted to 'true' when not provided. &lt;br&gt;&lt;br&gt;Minimum api-version: 2020-06-01.
        /// Serialized Name: DedicatedHostGroupUpdate.properties.supportAutomaticPlacement
        /// </summary>
        public bool? SupportAutomaticPlacement { get; set; }
    }
}
