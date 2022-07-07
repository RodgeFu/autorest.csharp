// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace Azure.ResourceManager.Sample.Models
{
    /// <summary>
    /// Describes a virtual machine scale set VM network profile.
    /// Serialized Name: VirtualMachineScaleSetVMNetworkProfileConfiguration
    /// </summary>
    internal partial class VirtualMachineScaleSetVMNetworkProfileConfiguration
    {
        /// <summary> Initializes a new instance of VirtualMachineScaleSetVMNetworkProfileConfiguration. </summary>
        public VirtualMachineScaleSetVMNetworkProfileConfiguration()
        {
            NetworkInterfaceConfigurations = new ChangeTrackingList<VirtualMachineScaleSetNetworkConfiguration>();
        }

        /// <summary> Initializes a new instance of VirtualMachineScaleSetVMNetworkProfileConfiguration. </summary>
        /// <param name="networkInterfaceConfigurations">
        /// The list of network configurations.
        /// Serialized Name: VirtualMachineScaleSetVMNetworkProfileConfiguration.networkInterfaceConfigurations
        /// </param>
        internal VirtualMachineScaleSetVMNetworkProfileConfiguration(IList<VirtualMachineScaleSetNetworkConfiguration> networkInterfaceConfigurations)
        {
            NetworkInterfaceConfigurations = networkInterfaceConfigurations;
        }

        /// <summary>
        /// The list of network configurations.
        /// Serialized Name: VirtualMachineScaleSetVMNetworkProfileConfiguration.networkInterfaceConfigurations
        /// </summary>
        public IList<VirtualMachineScaleSetNetworkConfiguration> NetworkInterfaceConfigurations { get; }
    }
}
