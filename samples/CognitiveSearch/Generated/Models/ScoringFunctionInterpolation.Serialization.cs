// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace CognitiveSearch.Models
{
    internal static partial class ScoringFunctionInterpolationExtensions
    {
        public static string ToSerialString(this ScoringFunctionInterpolation value) => value switch
        {
            ScoringFunctionInterpolation.Linear => "linear",
            ScoringFunctionInterpolation.Constant => "constant",
            ScoringFunctionInterpolation.Quadratic => "quadratic",
            ScoringFunctionInterpolation.Logarithmic => "logarithmic",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown ScoringFunctionInterpolation value.")
        };

        public static ScoringFunctionInterpolation ToScoringFunctionInterpolation(this string value)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(value, "linear")) return ScoringFunctionInterpolation.Linear;
            if (StringComparer.OrdinalIgnoreCase.Equals(value, "constant")) return ScoringFunctionInterpolation.Constant;
            if (StringComparer.OrdinalIgnoreCase.Equals(value, "quadratic")) return ScoringFunctionInterpolation.Quadratic;
            if (StringComparer.OrdinalIgnoreCase.Equals(value, "logarithmic")) return ScoringFunctionInterpolation.Logarithmic;
            throw new ArgumentOutOfRangeException(nameof(value), value, "Unknown ScoringFunctionInterpolation value.");
        }
    }
}
