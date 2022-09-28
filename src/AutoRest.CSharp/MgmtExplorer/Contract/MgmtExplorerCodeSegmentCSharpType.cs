// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoRest.CSharp.Generation.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    internal class MgmtExplorerCodeSegmentCSharpType
    {
        public string Name { get; }
        public string Namespace { get; }
        public bool IsValueType { get; }
        public bool IsEnum { get; }
        public bool IsNullable { get; }
        public bool isGenericType { get; }
        public List<MgmtExplorerCodeSegmentCSharpType> Arguments { get; }
        public string FullNameWithNamespace { get; }
        public string FullNameWithoutNamespace { get; }

        public MgmtExplorerCodeSegmentCSharpType(CSharpType csharpType)
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
            string name = includeNamespace ? $"{this.Namespace}.{this.Name}" : this.Name;
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
