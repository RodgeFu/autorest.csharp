// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace PetStore.Models
{
    /// <summary> Unknown version of Fish. </summary>
    internal partial class UnknownFish : Fish
    {
        /// <summary> Initializes a new instance of UnknownFish. </summary>
        /// <param name="size"> The size of the fish. </param>
        internal UnknownFish(int size) : base(size)
        {
        }

        /// <summary> Initializes a new instance of UnknownFish. </summary>
        /// <param name="kind"> Discriminator. </param>
        /// <param name="size"> The size of the fish. </param>
        internal UnknownFish(string kind, int size) : base(kind, size)
        {
        }
    }
}
