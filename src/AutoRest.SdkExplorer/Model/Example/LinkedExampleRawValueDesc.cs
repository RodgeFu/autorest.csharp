// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace AutoRest.SdkExplorer.Model.Example
{
    public class LinkedExampleRawValueDesc
    {
        // for yaml deserializer
        private LinkedExampleRawValueDesc() { }
        [JsonConstructor]
        public LinkedExampleRawValueDesc(string source, string value)
        {
            this.Source = source;
            this.Value = value;
        }

        public string? Value { get; set; }
        public string? Source { get; set; }
    }
}
