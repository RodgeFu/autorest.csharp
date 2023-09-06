// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;
using Azure.ResourceManager.Models;
using Azure.ResourceManager.Resources.Models;
using Azure.ResourceManager.Sample.Models;

namespace Azure.ResourceManager.Sample
{
    /// <summary>
    /// A class representing the VirtualMachine data model.
    /// Describes a Virtual Machine.
    /// Serialized Name: VirtualMachine
    /// </summary>
    public partial class VirtualMachineData : TrackedResourceData
    {
        /// <summary> Initializes a new instance of VirtualMachineData. </summary>
        /// <param name="location"> The location. </param>
        public VirtualMachineData(AzureLocation location) : base(location)
        {
            Resources = new ChangeTrackingList<VirtualMachineExtensionData>();
            Zones = new ChangeTrackingList<string>();
        }

        /// <summary> Initializes a new instance of VirtualMachineData. </summary>
        /// <param name="id"> The id. </param>
        /// <param name="name"> The name. </param>
        /// <param name="resourceType"> The resourceType. </param>
        /// <param name="systemData"> The systemData. </param>
        /// <param name="tags"> The tags. </param>
        /// <param name="location"> The location. </param>
        /// <param name="plan">
        /// Specifies information about the marketplace image used to create the virtual machine. This element is only used for marketplace images. Before you can use a marketplace image from an API, you must enable the image for programmatic use.  In the Azure portal, find the marketplace image that you want to use and then click **Want to deploy programmatically, Get Started -&gt;**. Enter any required information and then click **Save**.
        /// Serialized Name: VirtualMachine.plan
        /// </param>
        /// <param name="resources">
        /// The virtual machine child extension resources.
        /// Serialized Name: VirtualMachine.resources
        /// </param>
        /// <param name="identity">
        /// The identity of the virtual machine, if configured.
        /// Serialized Name: VirtualMachine.identity
        /// </param>
        /// <param name="zones">
        /// The virtual machine zones.
        /// Serialized Name: VirtualMachine.zones
        /// </param>
        /// <param name="hardwareProfile">
        /// Specifies the hardware settings for the virtual machine.
        /// Serialized Name: VirtualMachine.properties.hardwareProfile
        /// </param>
        /// <param name="storageProfile">
        /// Specifies the storage settings for the virtual machine disks.
        /// Serialized Name: VirtualMachine.properties.storageProfile
        /// </param>
        /// <param name="additionalCapabilities">
        /// Specifies additional capabilities enabled or disabled on the virtual machine.
        /// Serialized Name: VirtualMachine.properties.additionalCapabilities
        /// </param>
        /// <param name="osProfile">
        /// Specifies the operating system settings used while creating the virtual machine. Some of the settings cannot be changed once VM is provisioned.
        /// Serialized Name: VirtualMachine.properties.osProfile
        /// </param>
        /// <param name="networkProfile">
        /// Specifies the network interfaces of the virtual machine.
        /// Serialized Name: VirtualMachine.properties.networkProfile
        /// </param>
        /// <param name="securityProfile">
        /// Specifies the Security related profile settings for the virtual machine.
        /// Serialized Name: VirtualMachine.properties.securityProfile
        /// </param>
        /// <param name="diagnosticsProfile">
        /// Specifies the boot diagnostic settings state. &lt;br&gt;&lt;br&gt;Minimum api-version: 2015-06-15.
        /// Serialized Name: VirtualMachine.properties.diagnosticsProfile
        /// </param>
        /// <param name="availabilitySet">
        /// Specifies information about the availability set that the virtual machine should be assigned to. Virtual machines specified in the same availability set are allocated to different nodes to maximize availability. For more information about availability sets, see [Manage the availability of virtual machines](https://docs.microsoft.com/azure/virtual-machines/virtual-machines-windows-manage-availability?toc=%2fazure%2fvirtual-machines%2fwindows%2ftoc.json). &lt;br&gt;&lt;br&gt; For more information on Azure planned maintenance, see [Planned maintenance for virtual machines in Azure](https://docs.microsoft.com/azure/virtual-machines/virtual-machines-windows-planned-maintenance?toc=%2fazure%2fvirtual-machines%2fwindows%2ftoc.json) &lt;br&gt;&lt;br&gt; Currently, a VM can only be added to availability set at creation time. The availability set to which the VM is being added should be under the same resource group as the availability set resource. An existing VM cannot be added to an availability set. &lt;br&gt;&lt;br&gt;This property cannot exist along with a non-null properties.virtualMachineScaleSet reference.
        /// Serialized Name: VirtualMachine.properties.availabilitySet
        /// </param>
        /// <param name="virtualMachineScaleSet">
        /// Specifies information about the virtual machine scale set that the virtual machine should be assigned to. Virtual machines specified in the same virtual machine scale set are allocated to different nodes to maximize availability. Currently, a VM can only be added to virtual machine scale set at creation time. An existing VM cannot be added to a virtual machine scale set. &lt;br&gt;&lt;br&gt;This property cannot exist along with a non-null properties.availabilitySet reference. &lt;br&gt;&lt;br&gt;Minimum api‐version: 2019‐03‐01
        /// Serialized Name: VirtualMachine.properties.virtualMachineScaleSet
        /// </param>
        /// <param name="proximityPlacementGroup">
        /// Specifies information about the proximity placement group that the virtual machine should be assigned to. &lt;br&gt;&lt;br&gt;Minimum api-version: 2018-04-01.
        /// Serialized Name: VirtualMachine.properties.proximityPlacementGroup
        /// </param>
        /// <param name="priority">
        /// Specifies the priority for the virtual machine. &lt;br&gt;&lt;br&gt;Minimum api-version: 2019-03-01
        /// Serialized Name: VirtualMachine.properties.priority
        /// </param>
        /// <param name="evictionPolicy">
        /// Specifies the eviction policy for the Azure Spot virtual machine and Azure Spot scale set. &lt;br&gt;&lt;br&gt;For Azure Spot virtual machines, both 'Deallocate' and 'Delete' are supported and the minimum api-version is 2019-03-01. &lt;br&gt;&lt;br&gt;For Azure Spot scale sets, both 'Deallocate' and 'Delete' are supported and the minimum api-version is 2017-10-30-preview.
        /// Serialized Name: VirtualMachine.properties.evictionPolicy
        /// </param>
        /// <param name="billingProfile">
        /// Specifies the billing related details of a Azure Spot virtual machine. &lt;br&gt;&lt;br&gt;Minimum api-version: 2019-03-01.
        /// Serialized Name: VirtualMachine.properties.billingProfile
        /// </param>
        /// <param name="host">
        /// Specifies information about the dedicated host that the virtual machine resides in. &lt;br&gt;&lt;br&gt;Minimum api-version: 2018-10-01.
        /// Serialized Name: VirtualMachine.properties.host
        /// </param>
        /// <param name="hostGroup">
        /// Specifies information about the dedicated host group that the virtual machine resides in. &lt;br&gt;&lt;br&gt;Minimum api-version: 2020-06-01. &lt;br&gt;&lt;br&gt;NOTE: User cannot specify both host and hostGroup properties.
        /// Serialized Name: VirtualMachine.properties.hostGroup
        /// </param>
        /// <param name="provisioningState">
        /// The provisioning state, which only appears in the response.
        /// Serialized Name: VirtualMachine.properties.provisioningState
        /// </param>
        /// <param name="instanceView">
        /// The virtual machine instance view.
        /// Serialized Name: VirtualMachine.properties.instanceView
        /// </param>
        /// <param name="licenseType">
        /// Specifies that the image or disk that is being used was licensed on-premises. This element is only used for images that contain the Windows Server operating system. &lt;br&gt;&lt;br&gt; Possible values are: &lt;br&gt;&lt;br&gt; Windows_Client &lt;br&gt;&lt;br&gt; Windows_Server &lt;br&gt;&lt;br&gt; If this element is included in a request for an update, the value must match the initial value. This value cannot be updated. &lt;br&gt;&lt;br&gt; For more information, see [Azure Hybrid Use Benefit for Windows Server](https://docs.microsoft.com/azure/virtual-machines/virtual-machines-windows-hybrid-use-benefit-licensing?toc=%2fazure%2fvirtual-machines%2fwindows%2ftoc.json) &lt;br&gt;&lt;br&gt; Minimum api-version: 2015-06-15
        /// Serialized Name: VirtualMachine.properties.licenseType
        /// </param>
        /// <param name="vmId">
        /// Specifies the VM unique ID which is a 128-bits identifier that is encoded and stored in all Azure IaaS VMs SMBIOS and can be read using platform BIOS commands.
        /// Serialized Name: VirtualMachine.properties.vmId
        /// </param>
        /// <param name="extensionsTimeBudget">
        /// Specifies the time alloted for all extensions to start. The time duration should be between 15 minutes and 120 minutes (inclusive) and should be specified in ISO 8601 format. The default value is 90 minutes (PT1H30M). &lt;br&gt;&lt;br&gt; Minimum api-version: 2020-06-01
        /// Serialized Name: VirtualMachine.properties.extensionsTimeBudget
        /// </param>
        internal VirtualMachineData(ResourceIdentifier id, string name, ResourceType resourceType, SystemData systemData, IDictionary<string, string> tags, AzureLocation location, SamplePlan plan, IReadOnlyList<VirtualMachineExtensionData> resources, ManagedServiceIdentity identity, IList<string> zones, HardwareProfile hardwareProfile, StorageProfile storageProfile, AdditionalCapabilities additionalCapabilities, OSProfile osProfile, NetworkProfile networkProfile, SecurityProfile securityProfile, DiagnosticsProfile diagnosticsProfile, WritableSubResource availabilitySet, WritableSubResource virtualMachineScaleSet, WritableSubResource proximityPlacementGroup, VirtualMachinePriorityType? priority, VirtualMachineEvictionPolicyType? evictionPolicy, BillingProfile billingProfile, WritableSubResource host, WritableSubResource hostGroup, string provisioningState, VirtualMachineInstanceView instanceView, string licenseType, string vmId, string extensionsTimeBudget) : base(id, name, resourceType, systemData, tags, location)
        {
            Plan = plan;
            Resources = resources;
            Identity = identity;
            Zones = zones;
            HardwareProfile = hardwareProfile;
            StorageProfile = storageProfile;
            AdditionalCapabilities = additionalCapabilities;
            OSProfile = osProfile;
            NetworkProfile = networkProfile;
            SecurityProfile = securityProfile;
            DiagnosticsProfile = diagnosticsProfile;
            AvailabilitySet = availabilitySet;
            VirtualMachineScaleSet = virtualMachineScaleSet;
            ProximityPlacementGroup = proximityPlacementGroup;
            Priority = priority;
            EvictionPolicy = evictionPolicy;
            BillingProfile = billingProfile;
            Host = host;
            HostGroup = hostGroup;
            ProvisioningState = provisioningState;
            InstanceView = instanceView;
            LicenseType = licenseType;
            VmId = vmId;
            ExtensionsTimeBudget = extensionsTimeBudget;
        }

        /// <summary>
        /// Specifies information about the marketplace image used to create the virtual machine. This element is only used for marketplace images. Before you can use a marketplace image from an API, you must enable the image for programmatic use.  In the Azure portal, find the marketplace image that you want to use and then click **Want to deploy programmatically, Get Started -&gt;**. Enter any required information and then click **Save**.
        /// Serialized Name: VirtualMachine.plan
        /// </summary>
        public SamplePlan Plan { get; set; }
        /// <summary>
        /// The virtual machine child extension resources.
        /// Serialized Name: VirtualMachine.resources
        /// </summary>
        public IReadOnlyList<VirtualMachineExtensionData> Resources { get; }
        /// <summary>
        /// The identity of the virtual machine, if configured.
        /// Serialized Name: VirtualMachine.identity
        /// </summary>
        public ManagedServiceIdentity Identity { get; set; }
        /// <summary>
        /// The virtual machine zones.
        /// Serialized Name: VirtualMachine.zones
        /// </summary>
        public IList<string> Zones { get; }
        /// <summary>
        /// Specifies the hardware settings for the virtual machine.
        /// Serialized Name: VirtualMachine.properties.hardwareProfile
        /// </summary>
        internal HardwareProfile HardwareProfile { get; set; }
        /// <summary>
        /// Specifies the size of the virtual machine. For more information about virtual machine sizes, see [Sizes for virtual machines](https://docs.microsoft.com/azure/virtual-machines/virtual-machines-windows-sizes?toc=%2fazure%2fvirtual-machines%2fwindows%2ftoc.json). &lt;br&gt;&lt;br&gt; The available VM sizes depend on region and availability set. For a list of available sizes use these APIs:  &lt;br&gt;&lt;br&gt; [List all available virtual machine sizes in an availability set](https://docs.microsoft.com/rest/api/compute/availabilitysets/listavailablesizes) &lt;br&gt;&lt;br&gt; [List all available virtual machine sizes in a region](https://docs.microsoft.com/rest/api/compute/virtualmachinesizes/list) &lt;br&gt;&lt;br&gt; [List all available virtual machine sizes for resizing](https://docs.microsoft.com/rest/api/compute/virtualmachines/listavailablesizes)
        /// Serialized Name: HardwareProfile.vmSize
        /// </summary>
        public VirtualMachineSizeType? HardwareVmSize
        {
            get => HardwareProfile is null ? default : HardwareProfile.VmSize;
            set
            {
                if (HardwareProfile is null)
                    HardwareProfile = new HardwareProfile();
                HardwareProfile.VmSize = value;
            }
        }

        /// <summary>
        /// Specifies the storage settings for the virtual machine disks.
        /// Serialized Name: VirtualMachine.properties.storageProfile
        /// </summary>
        public StorageProfile StorageProfile { get; set; }
        /// <summary>
        /// Specifies additional capabilities enabled or disabled on the virtual machine.
        /// Serialized Name: VirtualMachine.properties.additionalCapabilities
        /// </summary>
        internal AdditionalCapabilities AdditionalCapabilities { get; set; }
        /// <summary>
        /// The flag that enables or disables a capability to have one or more managed data disks with UltraSSD_LRS storage account type on the VM or VMSS. Managed disks with storage account type UltraSSD_LRS can be added to a virtual machine or virtual machine scale set only if this property is enabled.
        /// Serialized Name: AdditionalCapabilities.ultraSSDEnabled
        /// </summary>
        public bool? UltraSSDEnabled
        {
            get => AdditionalCapabilities is null ? default : AdditionalCapabilities.UltraSSDEnabled;
            set
            {
                if (AdditionalCapabilities is null)
                    AdditionalCapabilities = new AdditionalCapabilities();
                AdditionalCapabilities.UltraSSDEnabled = value;
            }
        }

        /// <summary>
        /// Specifies the operating system settings used while creating the virtual machine. Some of the settings cannot be changed once VM is provisioned.
        /// Serialized Name: VirtualMachine.properties.osProfile
        /// </summary>
        public OSProfile OSProfile { get; set; }
        /// <summary>
        /// Specifies the network interfaces of the virtual machine.
        /// Serialized Name: VirtualMachine.properties.networkProfile
        /// </summary>
        internal NetworkProfile NetworkProfile { get; set; }
        /// <summary>
        /// Specifies the list of resource Ids for the network interfaces associated with the virtual machine.
        /// Serialized Name: NetworkProfile.networkInterfaces
        /// </summary>
        public IList<NetworkInterfaceReference> NetworkInterfaces
        {
            get
            {
                if (NetworkProfile is null)
                    NetworkProfile = new NetworkProfile();
                return NetworkProfile.NetworkInterfaces;
            }
        }

        /// <summary>
        /// Specifies the Security related profile settings for the virtual machine.
        /// Serialized Name: VirtualMachine.properties.securityProfile
        /// </summary>
        internal SecurityProfile SecurityProfile { get; set; }
        /// <summary>
        /// This property can be used by user in the request to enable or disable the Host Encryption for the virtual machine or virtual machine scale set. This will enable the encryption for all the disks including Resource/Temp disk at host itself. &lt;br&gt;&lt;br&gt; Default: The Encryption at host will be disabled unless this property is set to true for the resource.
        /// Serialized Name: SecurityProfile.encryptionAtHost
        /// </summary>
        public bool? EncryptionAtHost
        {
            get => SecurityProfile is null ? default : SecurityProfile.EncryptionAtHost;
            set
            {
                if (SecurityProfile is null)
                    SecurityProfile = new SecurityProfile();
                SecurityProfile.EncryptionAtHost = value;
            }
        }

        /// <summary>
        /// Specifies the boot diagnostic settings state. &lt;br&gt;&lt;br&gt;Minimum api-version: 2015-06-15.
        /// Serialized Name: VirtualMachine.properties.diagnosticsProfile
        /// </summary>
        internal DiagnosticsProfile DiagnosticsProfile { get; set; }
        /// <summary>
        /// Boot Diagnostics is a debugging feature which allows you to view Console Output and Screenshot to diagnose VM status. &lt;br&gt;&lt;br&gt; You can easily view the output of your console log. &lt;br&gt;&lt;br&gt; Azure also enables you to see a screenshot of the VM from the hypervisor.
        /// Serialized Name: DiagnosticsProfile.bootDiagnostics
        /// </summary>
        public BootDiagnostics BootDiagnostics
        {
            get => DiagnosticsProfile is null ? default : DiagnosticsProfile.BootDiagnostics;
            set
            {
                if (DiagnosticsProfile is null)
                    DiagnosticsProfile = new DiagnosticsProfile();
                DiagnosticsProfile.BootDiagnostics = value;
            }
        }

        /// <summary>
        /// Specifies information about the availability set that the virtual machine should be assigned to. Virtual machines specified in the same availability set are allocated to different nodes to maximize availability. For more information about availability sets, see [Manage the availability of virtual machines](https://docs.microsoft.com/azure/virtual-machines/virtual-machines-windows-manage-availability?toc=%2fazure%2fvirtual-machines%2fwindows%2ftoc.json). &lt;br&gt;&lt;br&gt; For more information on Azure planned maintenance, see [Planned maintenance for virtual machines in Azure](https://docs.microsoft.com/azure/virtual-machines/virtual-machines-windows-planned-maintenance?toc=%2fazure%2fvirtual-machines%2fwindows%2ftoc.json) &lt;br&gt;&lt;br&gt; Currently, a VM can only be added to availability set at creation time. The availability set to which the VM is being added should be under the same resource group as the availability set resource. An existing VM cannot be added to an availability set. &lt;br&gt;&lt;br&gt;This property cannot exist along with a non-null properties.virtualMachineScaleSet reference.
        /// Serialized Name: VirtualMachine.properties.availabilitySet
        /// </summary>
        internal WritableSubResource AvailabilitySet { get; set; }
        /// <summary> Gets or sets Id. </summary>
        public ResourceIdentifier AvailabilitySetId
        {
            get => AvailabilitySet is null ? default : AvailabilitySet.Id;
            set
            {
                if (AvailabilitySet is null)
                    AvailabilitySet = new WritableSubResource();
                AvailabilitySet.Id = value;
            }
        }

        /// <summary>
        /// Specifies information about the virtual machine scale set that the virtual machine should be assigned to. Virtual machines specified in the same virtual machine scale set are allocated to different nodes to maximize availability. Currently, a VM can only be added to virtual machine scale set at creation time. An existing VM cannot be added to a virtual machine scale set. &lt;br&gt;&lt;br&gt;This property cannot exist along with a non-null properties.availabilitySet reference. &lt;br&gt;&lt;br&gt;Minimum api‐version: 2019‐03‐01
        /// Serialized Name: VirtualMachine.properties.virtualMachineScaleSet
        /// </summary>
        internal WritableSubResource VirtualMachineScaleSet { get; set; }
        /// <summary> Gets or sets Id. </summary>
        public ResourceIdentifier VirtualMachineScaleSetId
        {
            get => VirtualMachineScaleSet is null ? default : VirtualMachineScaleSet.Id;
            set
            {
                if (VirtualMachineScaleSet is null)
                    VirtualMachineScaleSet = new WritableSubResource();
                VirtualMachineScaleSet.Id = value;
            }
        }

        /// <summary>
        /// Specifies information about the proximity placement group that the virtual machine should be assigned to. &lt;br&gt;&lt;br&gt;Minimum api-version: 2018-04-01.
        /// Serialized Name: VirtualMachine.properties.proximityPlacementGroup
        /// </summary>
        internal WritableSubResource ProximityPlacementGroup { get; set; }
        /// <summary> Gets or sets Id. </summary>
        public ResourceIdentifier ProximityPlacementGroupId
        {
            get => ProximityPlacementGroup is null ? default : ProximityPlacementGroup.Id;
            set
            {
                if (ProximityPlacementGroup is null)
                    ProximityPlacementGroup = new WritableSubResource();
                ProximityPlacementGroup.Id = value;
            }
        }

        /// <summary>
        /// Specifies the priority for the virtual machine. &lt;br&gt;&lt;br&gt;Minimum api-version: 2019-03-01
        /// Serialized Name: VirtualMachine.properties.priority
        /// </summary>
        public VirtualMachinePriorityType? Priority { get; set; }
        /// <summary>
        /// Specifies the eviction policy for the Azure Spot virtual machine and Azure Spot scale set. &lt;br&gt;&lt;br&gt;For Azure Spot virtual machines, both 'Deallocate' and 'Delete' are supported and the minimum api-version is 2019-03-01. &lt;br&gt;&lt;br&gt;For Azure Spot scale sets, both 'Deallocate' and 'Delete' are supported and the minimum api-version is 2017-10-30-preview.
        /// Serialized Name: VirtualMachine.properties.evictionPolicy
        /// </summary>
        public VirtualMachineEvictionPolicyType? EvictionPolicy { get; set; }
        /// <summary>
        /// Specifies the billing related details of a Azure Spot virtual machine. &lt;br&gt;&lt;br&gt;Minimum api-version: 2019-03-01.
        /// Serialized Name: VirtualMachine.properties.billingProfile
        /// </summary>
        internal BillingProfile BillingProfile { get; set; }
        /// <summary>
        /// Specifies the maximum price you are willing to pay for a Azure Spot VM/VMSS. This price is in US Dollars. &lt;br&gt;&lt;br&gt; This price will be compared with the current Azure Spot price for the VM size. Also, the prices are compared at the time of create/update of Azure Spot VM/VMSS and the operation will only succeed if  the maxPrice is greater than the current Azure Spot price. &lt;br&gt;&lt;br&gt; The maxPrice will also be used for evicting a Azure Spot VM/VMSS if the current Azure Spot price goes beyond the maxPrice after creation of VM/VMSS. &lt;br&gt;&lt;br&gt; Possible values are: &lt;br&gt;&lt;br&gt; - Any decimal value greater than zero. Example: 0.01538 &lt;br&gt;&lt;br&gt; -1 – indicates default price to be up-to on-demand. &lt;br&gt;&lt;br&gt; You can set the maxPrice to -1 to indicate that the Azure Spot VM/VMSS should not be evicted for price reasons. Also, the default max price is -1 if it is not provided by you. &lt;br&gt;&lt;br&gt;Minimum api-version: 2019-03-01.
        /// Serialized Name: BillingProfile.maxPrice
        /// </summary>
        public double? BillingMaxPrice
        {
            get => BillingProfile is null ? default : BillingProfile.MaxPrice;
            set
            {
                if (BillingProfile is null)
                    BillingProfile = new BillingProfile();
                BillingProfile.MaxPrice = value;
            }
        }

        /// <summary>
        /// Specifies information about the dedicated host that the virtual machine resides in. &lt;br&gt;&lt;br&gt;Minimum api-version: 2018-10-01.
        /// Serialized Name: VirtualMachine.properties.host
        /// </summary>
        internal WritableSubResource Host { get; set; }
        /// <summary> Gets or sets Id. </summary>
        public ResourceIdentifier HostId
        {
            get => Host is null ? default : Host.Id;
            set
            {
                if (Host is null)
                    Host = new WritableSubResource();
                Host.Id = value;
            }
        }

        /// <summary>
        /// Specifies information about the dedicated host group that the virtual machine resides in. &lt;br&gt;&lt;br&gt;Minimum api-version: 2020-06-01. &lt;br&gt;&lt;br&gt;NOTE: User cannot specify both host and hostGroup properties.
        /// Serialized Name: VirtualMachine.properties.hostGroup
        /// </summary>
        internal WritableSubResource HostGroup { get; set; }
        /// <summary> Gets or sets Id. </summary>
        public ResourceIdentifier HostGroupId
        {
            get => HostGroup is null ? default : HostGroup.Id;
            set
            {
                if (HostGroup is null)
                    HostGroup = new WritableSubResource();
                HostGroup.Id = value;
            }
        }

        /// <summary>
        /// The provisioning state, which only appears in the response.
        /// Serialized Name: VirtualMachine.properties.provisioningState
        /// </summary>
        public string ProvisioningState { get; }
        /// <summary>
        /// The virtual machine instance view.
        /// Serialized Name: VirtualMachine.properties.instanceView
        /// </summary>
        public VirtualMachineInstanceView InstanceView { get; }
        /// <summary>
        /// Specifies that the image or disk that is being used was licensed on-premises. This element is only used for images that contain the Windows Server operating system. &lt;br&gt;&lt;br&gt; Possible values are: &lt;br&gt;&lt;br&gt; Windows_Client &lt;br&gt;&lt;br&gt; Windows_Server &lt;br&gt;&lt;br&gt; If this element is included in a request for an update, the value must match the initial value. This value cannot be updated. &lt;br&gt;&lt;br&gt; For more information, see [Azure Hybrid Use Benefit for Windows Server](https://docs.microsoft.com/azure/virtual-machines/virtual-machines-windows-hybrid-use-benefit-licensing?toc=%2fazure%2fvirtual-machines%2fwindows%2ftoc.json) &lt;br&gt;&lt;br&gt; Minimum api-version: 2015-06-15
        /// Serialized Name: VirtualMachine.properties.licenseType
        /// </summary>
        public string LicenseType { get; set; }
        /// <summary>
        /// Specifies the VM unique ID which is a 128-bits identifier that is encoded and stored in all Azure IaaS VMs SMBIOS and can be read using platform BIOS commands.
        /// Serialized Name: VirtualMachine.properties.vmId
        /// </summary>
        public string VmId { get; }
        /// <summary>
        /// Specifies the time alloted for all extensions to start. The time duration should be between 15 minutes and 120 minutes (inclusive) and should be specified in ISO 8601 format. The default value is 90 minutes (PT1H30M). &lt;br&gt;&lt;br&gt; Minimum api-version: 2020-06-01
        /// Serialized Name: VirtualMachine.properties.extensionsTimeBudget
        /// </summary>
        public string ExtensionsTimeBudget { get; set; }
    }
}
