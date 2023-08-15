// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace AutoRest.SdkExplorer.Model.Code
{
    public class PlaceHolderDesc
    {
        public string? Key { get; set; }
        public string? SuggestedName { get; set; }

        protected PlaceHolderDesc()
        {
        }

        public PlaceHolderDesc(string? key, string? suggestedName)
        {
            this.Key = key;
            this.SuggestedName = suggestedName;
        }

        /// <summary>
        /// apply name to code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name">keep null to use suggestedName</param>
        public string Apply(string code, string? name = null)
        {
            if (string.IsNullOrEmpty(this.Key))
                return code;
            return code.Replace(this.Key, name ?? this.SuggestedName);
        }
    }
}
