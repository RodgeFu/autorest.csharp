// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using AutoRest.CSharp.Input;
using System.Linq;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerExampleValue
    {
        public string? RawValue { get; set; }
        public Dictionary<string, MgmtExplorerExampleValue>? PropertyValues { get; set; }
        public List<MgmtExplorerExampleValue>? ArrayValues { get; set; }

        public MgmtExplorerExampleValue()
        {

        }

        internal MgmtExplorerExampleValue(ExampleValue ev)
        {
            this.RawValue = ev.RawValue?.ToString();
            this.PropertyValues = ev.Properties?.ToDictionary(
                v => v.Key,
                v => new MgmtExplorerExampleValue(v.Value));
            this.ArrayValues = ev.Elements.Select(v => new MgmtExplorerExampleValue(v)).ToList();
        }
    }
}
