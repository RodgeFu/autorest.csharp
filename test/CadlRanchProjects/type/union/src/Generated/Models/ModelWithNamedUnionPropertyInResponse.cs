// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using Azure.Core;

namespace _Type.Union.Models
{
    /// <summary> The ModelWithNamedUnionPropertyInResponse. </summary>
    internal partial class ModelWithNamedUnionPropertyInResponse
    {
        /// <summary> Initializes a new instance of ModelWithNamedUnionPropertyInResponse. </summary>
        /// <param name="namedUnion"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="namedUnion"/> is null. </exception>
        internal ModelWithNamedUnionPropertyInResponse(object namedUnion)
        {
            Argument.AssertNotNull(namedUnion, nameof(namedUnion));

            NamedUnion = namedUnion;
        }

        /// <summary> Gets the named union. </summary>
        public object NamedUnion { get; }
    }
}
