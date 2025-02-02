// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MgmtCustomizations.Models
{
    public partial class PetStoreProperties : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Order))
            {
                writer.WritePropertyName("order"u8);
                writer.WriteNumberValue(Order.Value);
            }
            if (Optional.IsDefined(Pet))
            {
                writer.WritePropertyName("pet"u8);
                writer.WriteObjectValue(Pet);
            }
            writer.WriteEndObject();
        }

        internal static PetStoreProperties DeserializePetStoreProperties(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null)
            {
                return null;
            }
            Optional<int> order = default;
            Optional<Pet> pet = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("order"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    order = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("pet"u8))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    pet = Pet.DeserializePet(property.Value);
                    continue;
                }
            }
            return new PetStoreProperties(Optional.ToNullable(order), pet.Value);
        }
    }
}
