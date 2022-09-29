// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using AutoRest.CSharp.Generation.Types;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoRest.CSharp.MgmtExplorer.Autorest
{
    internal static class MgmtExplorerExtensions
    {
        public static string GetFullName(this CSharpType type, bool includeNamespace)
        {
            string name = includeNamespace ? $"{type.Namespace}.{type.Name}" : type.Name;
            if (type.IsNullable)
                name += "?";
            if (type.IsGenericType)
            {
                name += "<" + string.Join(", ", type.Arguments.Select(a => a.GetFullName(includeNamespace))) + ">";
            }
            return name;
        }
    }
}
