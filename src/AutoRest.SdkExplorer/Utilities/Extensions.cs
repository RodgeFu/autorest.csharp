// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AutoRest.SdkExplorer.Utilities
{
    public static class Extensions
    {
        public static string SerializeToYaml(this object obj)
        {
            var builder = new YamlDotNet.Serialization.SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var v = builder.Serialize(obj);
            return v;
        }

        public static T DeserializeYaml<T>(this string yaml)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample ymal
                .EnablePrivateConstructors()
                .Build();

            return deserializer.Deserialize<T>(yaml);
        }

        public static string SerializeToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            });
        }

        public static T? DeserializeJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }
    }
}
