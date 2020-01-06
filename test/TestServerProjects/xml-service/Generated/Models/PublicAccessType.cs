// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel;

namespace xml_service.Models.V100
{
    /// <summary> MISSING·SCHEMA-DESCRIPTION-CHOICE. </summary>
    public readonly partial struct PublicAccessType : IEquatable<PublicAccessType>
    {
        private readonly string? _value;

        /// <summary> Determines if two <see cref="PublicAccessType"/> values are the same. </summary>
        public PublicAccessType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string ContainerValue = "container";
        private const string BlobValue = "blob";

        /// <summary> container. </summary>
        public static PublicAccessType Container { get; } = new PublicAccessType(ContainerValue);
        /// <summary> blob. </summary>
        public static PublicAccessType Blob { get; } = new PublicAccessType(BlobValue);
        /// <summary> Determines if two <see cref="PublicAccessType"/> values are the same. </summary>
        public static bool operator ==(PublicAccessType left, PublicAccessType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="PublicAccessType"/> values are not the same. </summary>
        public static bool operator !=(PublicAccessType left, PublicAccessType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="PublicAccessType"/>. </summary>
        public static implicit operator PublicAccessType(string value) => new PublicAccessType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object? obj) => obj is PublicAccessType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(PublicAccessType other) => string.Equals(_value, other._value, StringComparison.Ordinal);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string? ToString() => _value;
    }
}
