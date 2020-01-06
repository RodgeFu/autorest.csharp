// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel;

namespace extensible_enums_swagger.Models.V20160707
{
    /// <summary> Type of Pet. </summary>
    public readonly partial struct DaysOfWeekExtensibleEnum : IEquatable<DaysOfWeekExtensibleEnum>
    {
        private readonly string? _value;

        /// <summary> Determines if two <see cref="DaysOfWeekExtensibleEnum"/> values are the same. </summary>
        public DaysOfWeekExtensibleEnum(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string MondayValue = "Monday";
        private const string TuesdayValue = "Tuesday";
        private const string WednesdayValue = "Wednesday";
        private const string ThursdayValue = "Thursday";
        private const string FridayValue = "Friday";
        private const string SaturdayValue = "Saturday";
        private const string SundayValue = "Sunday";

        /// <summary> Monday. </summary>
        public static DaysOfWeekExtensibleEnum Monday { get; } = new DaysOfWeekExtensibleEnum(MondayValue);
        /// <summary> Tuesday. </summary>
        public static DaysOfWeekExtensibleEnum Tuesday { get; } = new DaysOfWeekExtensibleEnum(TuesdayValue);
        /// <summary> Wednesday. </summary>
        public static DaysOfWeekExtensibleEnum Wednesday { get; } = new DaysOfWeekExtensibleEnum(WednesdayValue);
        /// <summary> Thursday. </summary>
        public static DaysOfWeekExtensibleEnum Thursday { get; } = new DaysOfWeekExtensibleEnum(ThursdayValue);
        /// <summary> Friday. </summary>
        public static DaysOfWeekExtensibleEnum Friday { get; } = new DaysOfWeekExtensibleEnum(FridayValue);
        /// <summary> Saturday. </summary>
        public static DaysOfWeekExtensibleEnum Saturday { get; } = new DaysOfWeekExtensibleEnum(SaturdayValue);
        /// <summary> Sunday. </summary>
        public static DaysOfWeekExtensibleEnum Sunday { get; } = new DaysOfWeekExtensibleEnum(SundayValue);
        /// <summary> Determines if two <see cref="DaysOfWeekExtensibleEnum"/> values are the same. </summary>
        public static bool operator ==(DaysOfWeekExtensibleEnum left, DaysOfWeekExtensibleEnum right) => left.Equals(right);
        /// <summary> Determines if two <see cref="DaysOfWeekExtensibleEnum"/> values are not the same. </summary>
        public static bool operator !=(DaysOfWeekExtensibleEnum left, DaysOfWeekExtensibleEnum right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="DaysOfWeekExtensibleEnum"/>. </summary>
        public static implicit operator DaysOfWeekExtensibleEnum(string value) => new DaysOfWeekExtensibleEnum(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object? obj) => obj is DaysOfWeekExtensibleEnum other && Equals(other);
        /// <inheritdoc />
        public bool Equals(DaysOfWeekExtensibleEnum other) => string.Equals(_value, other._value, StringComparison.Ordinal);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string? ToString() => _value;
    }
}
