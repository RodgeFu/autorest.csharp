// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace CognitiveSearch.Models
{
    public partial class WordDelimiterTokenFilter : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(GenerateWordParts))
            {
                writer.WritePropertyName("generateWordParts"u8);
                writer.WriteBooleanValue(GenerateWordParts.Value);
            }
            if (Optional.IsDefined(GenerateNumberParts))
            {
                writer.WritePropertyName("generateNumberParts"u8);
                writer.WriteBooleanValue(GenerateNumberParts.Value);
            }
            if (Optional.IsDefined(CatenateWords))
            {
                writer.WritePropertyName("catenateWords"u8);
                writer.WriteBooleanValue(CatenateWords.Value);
            }
            if (Optional.IsDefined(CatenateNumbers))
            {
                writer.WritePropertyName("catenateNumbers"u8);
                writer.WriteBooleanValue(CatenateNumbers.Value);
            }
            if (Optional.IsDefined(CatenateAll))
            {
                writer.WritePropertyName("catenateAll"u8);
                writer.WriteBooleanValue(CatenateAll.Value);
            }
            if (Optional.IsDefined(SplitOnCaseChange))
            {
                writer.WritePropertyName("splitOnCaseChange"u8);
                writer.WriteBooleanValue(SplitOnCaseChange.Value);
            }
            if (Optional.IsDefined(PreserveOriginal))
            {
                writer.WritePropertyName("preserveOriginal"u8);
                writer.WriteBooleanValue(PreserveOriginal.Value);
            }
            if (Optional.IsDefined(SplitOnNumerics))
            {
                writer.WritePropertyName("splitOnNumerics"u8);
                writer.WriteBooleanValue(SplitOnNumerics.Value);
            }
            if (Optional.IsDefined(StemEnglishPossessive))
            {
                writer.WritePropertyName("stemEnglishPossessive"u8);
                writer.WriteBooleanValue(StemEnglishPossessive.Value);
            }
            if (Optional.IsCollectionDefined(ProtectedWords))
            {
                writer.WritePropertyName("protectedWords"u8);
                writer.WriteStartArray();
                foreach (var item in ProtectedWords)
                {
                    writer.WriteStringValue(item);
                }
                writer.WriteEndArray();
            }
            writer.WritePropertyName("@odata.type"u8);
            writer.WriteStringValue(OdataType);
            writer.WritePropertyName("name"u8);
            writer.WriteStringValue(Name);
            writer.WriteEndObject();
        }

        internal static WordDelimiterTokenFilter DeserializeWordDelimiterTokenFilter(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            Optional<bool> generateWordParts = default;
            Optional<bool> generateNumberParts = default;
            Optional<bool> catenateWords = default;
            Optional<bool> catenateNumbers = default;
            Optional<bool> catenateAll = default;
            Optional<bool> splitOnCaseChange = default;
            Optional<bool> preserveOriginal = default;
            Optional<bool> splitOnNumerics = default;
            Optional<bool> stemEnglishPossessive = default;
            Optional<IList<string>> protectedWords = default;
            string odataType = default;
            string name = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("generateWordParts"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    generateWordParts = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("generateNumberParts"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    generateNumberParts = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("catenateWords"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    catenateWords = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("catenateNumbers"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    catenateNumbers = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("catenateAll"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    catenateAll = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("splitOnCaseChange"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    splitOnCaseChange = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("preserveOriginal"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    preserveOriginal = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("splitOnNumerics"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    splitOnNumerics = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("stemEnglishPossessive"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    stemEnglishPossessive = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("protectedWords"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    List<string> array = new List<string>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(item.GetString());
                    }
                    protectedWords = array;
                    continue;
                }
                if (property.NameEquals("@odata.type"u8))
                {
                    odataType = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("name"u8))
                {
                    name = property.Value.GetString();
                    continue;
                }
            }
            return new WordDelimiterTokenFilter(odataType, name, Optional.ToNullable(generateWordParts), Optional.ToNullable(generateNumberParts), Optional.ToNullable(catenateWords), Optional.ToNullable(catenateNumbers), Optional.ToNullable(catenateAll), Optional.ToNullable(splitOnCaseChange), Optional.ToNullable(preserveOriginal), Optional.ToNullable(splitOnNumerics), Optional.ToNullable(stemEnglishPossessive), Optional.ToList(protectedWords));
        }
    }
}
