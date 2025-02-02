// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace xms_error_responses.Models
{
    /// <summary> The UnknownNotFoundErrorBase. </summary>
    internal partial class UnknownNotFoundErrorBase : NotFoundErrorBase
    {
        /// <summary> Initializes a new instance of <see cref="UnknownNotFoundErrorBase"/>. </summary>
        /// <param name="someBaseProp"></param>
        /// <param name="reason"></param>
        /// <param name="whatNotFound"></param>
        internal UnknownNotFoundErrorBase(string someBaseProp, string reason, string whatNotFound) : base(someBaseProp, reason, whatNotFound)
        {
            WhatNotFound = whatNotFound ?? "Unknown";
        }
    }
}
