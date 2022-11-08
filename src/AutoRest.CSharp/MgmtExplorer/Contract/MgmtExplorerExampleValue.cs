// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using AutoRest.CSharp.Input;
using System.Linq;
using AutoRest.CSharp.MgmtExplorer.Autorest;
using System.Collections;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerExampleValue
    {
        public string? SerializerName { get; set; }
        public string? ModelName { get; set; }
        public string? SchemaType { get; set; }
        public string? CSharpName { get; set; }
        public string? RawValue { get; set; }
        public Dictionary<string, MgmtExplorerExampleValue>? PropertyValues { get; set; }
        public List<MgmtExplorerExampleValue>? ArrayValues { get; set; }

        public MgmtExplorerExampleValue()
        {

        }

        internal MgmtExplorerExampleValue(ExampleValue ev)
        {
            this.SchemaType = ev.Schema.Type.ToString();
            if (ev.Language != null)
            {
                this.ModelName = ev.Language.GetName();
                this.SerializerName = ev.Language.GetSerializerNameOrName();
                this.CSharpName = ev.CSharpName();
            }

            if (this.SchemaType == AllSchemaTypes.AnyObject.ToString() || this.SchemaType == AllSchemaTypes.Any.ToString())
            {
                // we know nothing about it. just ignore these parameters for now because we dont know how to re-create these value later anyway
                // TODO: add some handling if we found some "Any" can be guessed...
                this.RawValue = null;
            }
            else
            {
                this.RawValue = ev.RawValue?.ToString();
            }
            this.PropertyValues = ev.Properties?.ToDictionary(
                v => v.Key,
                v => new MgmtExplorerExampleValue(v.Value));
            this.ArrayValues = ev.Elements.Select(v => new MgmtExplorerExampleValue(v)).ToList();

        }
    }
}
