// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace AutoRest.SdkExplorer.Model.Hint
{
    public class ApiHint
    {
        private List<FieldHint> FieldHintsInternal { get; set; } = new List<FieldHint>();
        public IReadOnlyCollection<FieldHint> FieldHints => this.FieldHintsInternal.AsReadOnly();

        // private ctor for yaml ser/der
        [JsonConstructor]
        private ApiHint(IReadOnlyCollection<FieldHint> propertyExDatas)
        {
            this.FieldHintsInternal = new List<FieldHint>();
            foreach (var item in propertyExDatas)
                this.AppendFieldHint(item);
        }

        public ApiHint()
        {
        }

        public void AppendFieldHint(FieldHint propertyExData)
        {
            var found = FieldHintsInternal.FirstOrDefault(p => p.Key == propertyExData.Key);
            if (found != null)
                found.Merge(propertyExData);
            else
                FieldHintsInternal.Add(propertyExData);
        }

        /// <summary>
        /// Merge the given ApiHint 'other' into current instance
        /// </summary>
        /// <param name="other"></param>
        public void Merge(ApiHint other)
        {
            foreach (var item in other.FieldHints)
                this.AppendFieldHint(item);
        }
    }
}
