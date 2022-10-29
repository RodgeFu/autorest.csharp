// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.CSharp.Generation.Types;

namespace AutoRest.CSharp.MgmtExplorer.Models
{
    internal class MgmtExplorerPlaceHolder
    {
        public string Key { get; init; }
        public CSharpType Type { get; init; }
        protected string? DefaultReplacement { get; init; }

        public MgmtExplorerPlaceHolder(string key, CSharpType type, string? defaultReplacement)
        {
            this.Key = key;
            this.Type = type;
            this.DefaultReplacement = defaultReplacement;
        }
    }
}
