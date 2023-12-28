// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Text;
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

        public static string EncodeToBase64String(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        public static string DecodeFromBase64String(this string base64Str)
        {
            var bytes = Convert.FromBase64String(base64Str);
            return Encoding.UTF8.GetString(bytes);
        }

        public static uint GetStableHashCode(this string str)
        {
            unchecked
            {
                int hash1 = 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1 || str[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                var r = hash1 + (hash2 * 1566083941);
                return (uint)r;
            }
        }
    }
}
