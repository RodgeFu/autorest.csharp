// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Text.Json;
using Azure;
using Azure.Core;

namespace AnomalyDetector.Models
{
    public partial class AnomalyDetectionModel : IUtf8JsonSerializable, IJsonModel<AnomalyDetectionModel>
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer) => ((IJsonModel<AnomalyDetectionModel>)this).Write(writer, new ModelReaderWriterOptions("W"));

        void IJsonModel<AnomalyDetectionModel>.Write(Utf8JsonWriter writer, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<AnomalyDetectionModel>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(AnomalyDetectionModel)} does not support writing '{format}' format.");
            }

            writer.WriteStartObject();
            if (options.Format != "W")
            {
                writer.WritePropertyName("modelId"u8);
                writer.WriteStringValue(ModelId);
            }
            writer.WritePropertyName("createdTime"u8);
            writer.WriteStringValue(CreatedTime, "O");
            writer.WritePropertyName("lastUpdatedTime"u8);
            writer.WriteStringValue(LastUpdatedTime, "O");
            if (Optional.IsDefined(ModelInfo))
            {
                writer.WritePropertyName("modelInfo"u8);
                writer.WriteObjectValue(ModelInfo);
            }
            if (options.Format != "W" && _serializedAdditionalRawData != null)
            {
                foreach (var item in _serializedAdditionalRawData)
                {
                    writer.WritePropertyName(item.Key);
#if NET6_0_OR_GREATER
				writer.WriteRawValue(item.Value);
#else
                    using (JsonDocument document = JsonDocument.Parse(item.Value))
                    {
                        JsonSerializer.Serialize(writer, document.RootElement);
                    }
#endif
                }
            }
            writer.WriteEndObject();
        }

        AnomalyDetectionModel IJsonModel<AnomalyDetectionModel>.Create(ref Utf8JsonReader reader, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<AnomalyDetectionModel>)this).GetFormatFromOptions(options) : options.Format;
            if (format != "J")
            {
                throw new FormatException($"The model {nameof(AnomalyDetectionModel)} does not support reading '{format}' format.");
            }

            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            return DeserializeAnomalyDetectionModel(document.RootElement, options);
        }

        internal static AnomalyDetectionModel DeserializeAnomalyDetectionModel(JsonElement element, ModelReaderWriterOptions options = null)
        {
            options ??= new ModelReaderWriterOptions("W");

            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            Guid modelId = default;
            DateTimeOffset createdTime = default;
            DateTimeOffset lastUpdatedTime = default;
            ModelInfo modelInfo = default;
            IDictionary<string, BinaryData> serializedAdditionalRawData = default;
            Dictionary<string, BinaryData> additionalPropertiesDictionary = new Dictionary<string, BinaryData>();
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("modelId"u8))
                {
                    modelId = property.Value.GetGuid();
                    continue;
                }
                if (property.NameEquals("createdTime"u8))
                {
                    createdTime = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("lastUpdatedTime"u8))
                {
                    lastUpdatedTime = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("modelInfo"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    modelInfo = ModelInfo.DeserializeModelInfo(property.Value, options);
                    continue;
                }
                if (options.Format != "W")
                {
                    additionalPropertiesDictionary.Add(property.Name, BinaryData.FromString(property.Value.GetRawText()));
                }
            }
            serializedAdditionalRawData = additionalPropertiesDictionary;
            return new AnomalyDetectionModel(modelId, createdTime, lastUpdatedTime, modelInfo, serializedAdditionalRawData);
        }

        BinaryData IPersistableModel<AnomalyDetectionModel>.Write(ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<AnomalyDetectionModel>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    return ModelReaderWriter.Write(this, options);
                default:
                    throw new FormatException($"The model {nameof(AnomalyDetectionModel)} does not support writing '{options.Format}' format.");
            }
        }

        AnomalyDetectionModel IPersistableModel<AnomalyDetectionModel>.Create(BinaryData data, ModelReaderWriterOptions options)
        {
            var format = options.Format == "W" ? ((IPersistableModel<AnomalyDetectionModel>)this).GetFormatFromOptions(options) : options.Format;

            switch (format)
            {
                case "J":
                    {
                        using JsonDocument document = JsonDocument.Parse(data);
                        return DeserializeAnomalyDetectionModel(document.RootElement, options);
                    }
                default:
                    throw new FormatException($"The model {nameof(AnomalyDetectionModel)} does not support reading '{options.Format}' format.");
            }
        }

        string IPersistableModel<AnomalyDetectionModel>.GetFormatFromOptions(ModelReaderWriterOptions options) => "J";

        /// <summary> Deserializes the model from a raw response. </summary>
        /// <param name="response"> The response to deserialize the model from. </param>
        internal static AnomalyDetectionModel FromResponse(Response response)
        {
            using var document = JsonDocument.Parse(response.Content);
            return DeserializeAnomalyDetectionModel(document.RootElement);
        }

        /// <summary> Convert into a Utf8JsonRequestContent. </summary>
        internal virtual RequestContent ToRequestContent()
        {
            var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(this);
            return content;
        }
    }
}
