// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoRest.CSharp.Generation.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerCodeSegmentCSharpType
    {
        public string? Name { get; set; }
        public string? Namespace { get; set; }
        public bool IsValueType { get; set; }
        public bool IsEnum { get; set; }
        public bool IsNullable { get; set; }
        public bool isGenericType { get; set; }
        public List<MgmtExplorerCodeSegmentCSharpType> Arguments { get; set; } = new List<MgmtExplorerCodeSegmentCSharpType>();
        public string? FullNameWithNamespace { get; set; }
        public string? FullNameWithoutNamespace { get; set; }

        public MgmtExplorerCodeSegmentCSharpType()
        {
        }

        internal MgmtExplorerCodeSegmentCSharpType(CSharpType csharpType)
        {
            this.Name = csharpType.Name;
            this.Namespace = csharpType.Namespace;
            this.IsValueType = csharpType.IsValueType;
            this.IsEnum = csharpType.IsEnum;
            this.IsNullable = csharpType.IsNullable;
            this.isGenericType = csharpType.IsGenericType;
            this.Arguments = new List<MgmtExplorerCodeSegmentCSharpType>();
            if (isGenericType)
            {
                foreach (var t in csharpType.Arguments)
                {
                    this.Arguments.Add(new MgmtExplorerCodeSegmentCSharpType(t));
                }
            }
            this.FullNameWithNamespace = GetFullName(true);
            this.FullNameWithoutNamespace = GetFullName(false);
        }

        private string GetFullName(bool includeNamespace)
        {
            string name = includeNamespace ? $"{this.Namespace ?? "__N/A__"}.{this.Name ?? "__N/A__"}" : this.Name ?? "__N/A__";
            if (IsNullable)
                name += "?";
            if (isGenericType)
            {
                name += "<" + string.Join(", ", this.Arguments.Select(a => a.GetFullName(includeNamespace))) + ">";
            }
            return name;
        }
    }
}
