// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Input;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using AutoRest.CSharp.Output.Models.Types;

namespace AutoRest.CSharp.MgmtExplorer.Autorest
{
    internal static class MgmtExplorerExtensions
    {
        public static string GetFullName(this CSharpType type, bool includeNamespace, bool includeNullable = true)
        {
            string name = includeNamespace ? $"{type.Namespace}.{type.Name}" : type.Name;
            if (includeNullable && type.IsNullable)
                name += "?";
            if (type.IsGenericType)
            {
                name += "<" + string.Join(", ", type.Arguments.Select(a => a.GetFullName(includeNamespace, true))) + ">";
            }
            return name;
        }

        public static string GetSerializerNameOrName(this ObjectTypeProperty prop)
        {
            return prop.SchemaProperty?.SerializedName ?? prop.InputModelProperty?.SerializedName ?? prop.InputModelProperty?.Name ?? prop.Declaration.Name;
        }

        public static string GetSerializerNameOrName(this Languages lang, string langName = "default")
        {
            var getResult = (Language l) => l.SerializedName ?? l.Name;
            switch (langName)
            {
                //  TODO: add more language support as needed
                case "dotnet":
                    return getResult(lang.CSharp ?? lang.Default);
                default:
                    return getResult(lang.Default);
            }
        }

        public static string GetName(this Languages lang, string langName = "default")
        {
            var getResult = (Language l) => l.Name;
            switch (langName)
            {
                //  TODO: add more language support as needed
                case "dotnet":
                    return getResult(lang.CSharp ?? lang.Default);
                default:
                    return getResult(lang.Default);
            }
        }
    }
}
