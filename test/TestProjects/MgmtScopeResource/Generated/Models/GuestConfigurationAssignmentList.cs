// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;
using MgmtScopeResource;

namespace MgmtScopeResource.Models
{
    /// <summary> The response of the list guest configuration assignment operation. </summary>
    internal partial class GuestConfigurationAssignmentList
    {
        /// <summary> Initializes a new instance of GuestConfigurationAssignmentList. </summary>
        internal GuestConfigurationAssignmentList()
        {
            Value = new ChangeTrackingList<GuestConfigurationAssignmentData>();
        }

        /// <summary> Initializes a new instance of GuestConfigurationAssignmentList. </summary>
        /// <param name="value"> Result of the list guest configuration assignment operation. </param>
        internal GuestConfigurationAssignmentList(IReadOnlyList<GuestConfigurationAssignmentData> value)
        {
            Value = value;
        }

        /// <summary> Result of the list guest configuration assignment operation. </summary>
        public IReadOnlyList<GuestConfigurationAssignmentData> Value { get; }
    }
}
