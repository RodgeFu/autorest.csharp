// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Input;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

        public static string ToYaml(this object obj)
        {
            var builder = new YamlDotNet.Serialization.SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var v = builder.Serialize(obj);
            return v;
        }

        public static T FromYaml<T>(string yaml)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml
                .Build();

            return deserializer.Deserialize<T>(yaml);
        }
    }
}
