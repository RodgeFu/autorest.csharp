// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;
using MgmtCustomizations;

namespace MgmtCustomizations.Models
{
    /// <summary> The list result of the rules. </summary>
    internal partial class PetStoreListResult
    {
        /// <summary> Initializes a new instance of PetStoreListResult. </summary>
        internal PetStoreListResult()
        {
            Value = new ChangeTrackingList<PetStoreData>();
        }

        /// <summary> Initializes a new instance of PetStoreListResult. </summary>
        /// <param name="value"> The values. </param>
        internal PetStoreListResult(IReadOnlyList<PetStoreData> value)
        {
            Value = value;
        }

        /// <summary> The values. </summary>
        public IReadOnlyList<PetStoreData> Value { get; }
    }
}
