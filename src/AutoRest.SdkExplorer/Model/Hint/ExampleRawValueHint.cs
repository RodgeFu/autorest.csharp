// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AutoRest.SdkExplorer.Model.Hint
{
    public class ExampleRawValueHint : IEquatable<ExampleRawValueHint?>
    {
        // for yaml deserializer
        private ExampleRawValueHint() { }
        [JsonConstructor]
        public ExampleRawValueHint(string source, string value)
        {
            Source = source;
            Value = value;
        }

        public string? Value { get; set; }
        public string? Source { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ExampleRawValueHint);
        }

        public bool Equals(ExampleRawValueHint? other)
        {
            return other is not null &&
                   Value == other.Value &&
                   Source == other.Source;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Source);
        }

        public static bool operator ==(ExampleRawValueHint? left, ExampleRawValueHint? right)
        {
            return EqualityComparer<ExampleRawValueHint>.Default.Equals(left, right);
        }

        public static bool operator !=(ExampleRawValueHint? left, ExampleRawValueHint? right)
        {
            return !(left == right);
        }
    }
}
