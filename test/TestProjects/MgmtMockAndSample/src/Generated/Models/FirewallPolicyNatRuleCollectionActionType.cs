// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MgmtMockAndSample.Models
{
    /// <summary> The action type of a rule. </summary>
    public readonly partial struct FirewallPolicyNatRuleCollectionActionType : IEquatable<FirewallPolicyNatRuleCollectionActionType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="FirewallPolicyNatRuleCollectionActionType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public FirewallPolicyNatRuleCollectionActionType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string DnatValue = "DNAT";

        /// <summary> DNAT. </summary>
        public static FirewallPolicyNatRuleCollectionActionType Dnat { get; } = new FirewallPolicyNatRuleCollectionActionType(DnatValue);
        /// <summary> Determines if two <see cref="FirewallPolicyNatRuleCollectionActionType"/> values are the same. </summary>
        public static bool operator ==(FirewallPolicyNatRuleCollectionActionType left, FirewallPolicyNatRuleCollectionActionType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="FirewallPolicyNatRuleCollectionActionType"/> values are not the same. </summary>
        public static bool operator !=(FirewallPolicyNatRuleCollectionActionType left, FirewallPolicyNatRuleCollectionActionType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="FirewallPolicyNatRuleCollectionActionType"/>. </summary>
        public static implicit operator FirewallPolicyNatRuleCollectionActionType(string value) => new FirewallPolicyNatRuleCollectionActionType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is FirewallPolicyNatRuleCollectionActionType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(FirewallPolicyNatRuleCollectionActionType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
