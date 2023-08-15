// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace AutoRest.SdkExplorer.Model.Schema
{
    public class SchemaMethod
    {
        public string Name { get; set; } = string.Empty;

        public List<SchemaMethodParameter> MethodParameters { get; set; } = new List<SchemaMethodParameter>();

        public SchemaMethod()
        {

        }

        public bool HasParameter(string parameterSerializerName)
        {
            return this.MethodParameters.Any(mp => mp.RelatedPropertySerializerPath == parameterSerializerName);
        }
    }
}
