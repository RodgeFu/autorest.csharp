// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using Azure.Core;

namespace MgmtScopeResource.Models
{
    /// <summary> Deployment What-if operation parameters. </summary>
    public partial class DeploymentWhatIf
    {
        /// <summary> Initializes a new instance of DeploymentWhatIf. </summary>
        /// <param name="properties"> The deployment properties. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="properties"/> is null. </exception>
        public DeploymentWhatIf(DeploymentWhatIfProperties properties)
        {
            Argument.AssertNotNull(properties, nameof(properties));

            Properties = properties;
        }

        /// <summary> The location to store the deployment data. </summary>
        public string Location { get; set; }
        /// <summary> The deployment properties. </summary>
        public DeploymentWhatIfProperties Properties { get; }
    }
}
