// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using Azure.Core;

namespace Models.Property.Types.Models
{
    /// <summary> Model with a string property. </summary>
    public partial class StringProperty
    {
        /// <summary> Initializes a new instance of StringProperty. </summary>
        /// <param name="property"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="property"/> is null. </exception>
        public StringProperty(string property)
        {
            Argument.AssertNotNull(property, nameof(property));

            Property = property;
        }

        /// <summary> Gets or sets the property. </summary>
        public string Property { get; set; }
    }
}
