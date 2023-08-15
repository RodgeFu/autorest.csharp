// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Text.Json.Serialization;
using AutoRest.SdkExplorer.Model.Code;
using AutoRest.SdkExplorer.Model.Example;
using YamlDotNet.Serialization;

namespace AutoRest.SdkExplorer.Model.Schema
{
    public class SchemaProperty
    {
        public string? Accessibility { get; set; }
        public TypeDesc? Type { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? SerializerPath { get; set; }
        public bool IsRequired { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsWritableThroughCtor { get; set; }

        [JsonIgnore, YamlIgnore]
        public bool IsFlattenedProperty => this.SerializerPath!.IndexOf("/") >= 0;

        public SchemaProperty()
        {
        }

        public string[] GetSerializerPathSegments()
        {
            return this.SerializerPath!.Split("/");
        }

        public ExampleValueDesc? FindMatchingExample(ExampleValueDesc ex)
        {
            if (this.IsFlattenedProperty)
            {
                var segs = this.GetSerializerPathSegments();

                ExampleValueDesc? curExample = ex;
                int curSeg = 0;
                if (ex.SerializerName != segs[curSeg])
                    return null;

                for (curSeg = curSeg + 1; curSeg < segs.Length; curSeg++)
                {
                    if (!curExample.HasPropertyValues)
                        return null;
                    curExample = curExample.PropertyValues!.FirstOrDefault(p => p.Value.SerializerName == segs[curSeg]).Value;
                    if (curExample == null)
                        return null;
                }
                return curExample;
            }
            else
            {
                if (this.SerializerPath == ex.SerializerName)
                    return ex;
                else
                    return null;
            }
        }
    }
}
