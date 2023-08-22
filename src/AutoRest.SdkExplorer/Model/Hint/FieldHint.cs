// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using AutoRest.SdkExplorer.Model.Azure;
using AutoRest.SdkExplorer.Model.Hint;

namespace AutoRest.SdkExplorer.Model.Hint
{
    public class FieldHint
    {
        public string Key { get; set; }
        public HashSet<AzureResourceType> AzureResourceTypes { get; set; } = new HashSet<AzureResourceType>();
        public HashSet<ExampleRawValueHint> RawExampleValues { get; set; } = new HashSet<ExampleRawValueHint>();

        public FieldHint(string key)
        {
            this.Key = key;
        }

        /// <summary>
        /// merge the given FieldHint 'other' into current instance
        /// </summary>
        /// <param name="other"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Merge(FieldHint other)
        {
            if (this.Key != other.Key)
                throw new InvalidOperationException($"Can't Merge with different key {this.Key} != {other.Key}");
            foreach (var item in other.AzureResourceTypes)
                this.AzureResourceTypes.Add(item);
            foreach (var item in other.RawExampleValues)
                this.RawExampleValues.Add(item);
        }
    }
}
