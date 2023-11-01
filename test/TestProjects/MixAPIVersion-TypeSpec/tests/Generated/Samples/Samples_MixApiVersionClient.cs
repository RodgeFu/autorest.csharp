// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Identity;
using MixApiVersion;
using NUnit.Framework;

namespace MixApiVersion.Samples
{
    public partial class Samples_MixApiVersionClient
    {
        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_Delete_ShortVersion()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            Response response = client.Delete("<name>");

            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_Delete_ShortVersion_Async()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            Response response = await client.DeleteAsync("<name>");

            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_Delete_AllParameters()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            Response response = client.Delete("<name>");

            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_Delete_AllParameters_Async()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            Response response = await client.DeleteAsync("<name>");

            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_Read_ShortVersion()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            Response response = client.Read(1234, null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("name").ToString());
            Console.WriteLine(result.GetProperty("age").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_Read_ShortVersion_Async()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            Response response = await client.ReadAsync(1234, null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("name").ToString());
            Console.WriteLine(result.GetProperty("age").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_Read_AllParameters()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            Response response = client.Read(1234, null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("name").ToString());
            Console.WriteLine(result.GetProperty("tag").ToString());
            Console.WriteLine(result.GetProperty("age").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_Read_AllParameters_Async()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            Response response = await client.ReadAsync(1234, null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("name").ToString());
            Console.WriteLine(result.GetProperty("tag").ToString());
            Console.WriteLine(result.GetProperty("age").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_Create_ShortVersion()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            using RequestContent content = RequestContent.Create(new
            {
                age = 1234,
            });
            Response response = client.Create(content);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("name").ToString());
            Console.WriteLine(result.GetProperty("age").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_Create_ShortVersion_Async()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            using RequestContent content = RequestContent.Create(new
            {
                age = 1234,
            });
            Response response = await client.CreateAsync(content);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("name").ToString());
            Console.WriteLine(result.GetProperty("age").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_Create_AllParameters()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            using RequestContent content = RequestContent.Create(new
            {
                tag = "<tag>",
                age = 1234,
            });
            Response response = client.Create(content);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("name").ToString());
            Console.WriteLine(result.GetProperty("tag").ToString());
            Console.WriteLine(result.GetProperty("age").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_Create_AllParameters_Async()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            using RequestContent content = RequestContent.Create(new
            {
                tag = "<tag>",
                age = 1234,
            });
            Response response = await client.CreateAsync(content);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("name").ToString());
            Console.WriteLine(result.GetProperty("tag").ToString());
            Console.WriteLine(result.GetProperty("age").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetPets_ShortVersion()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            foreach (BinaryData item in client.GetPets(null))
            {
                JsonElement result = JsonDocument.Parse(item.ToStream()).RootElement;
                Console.WriteLine(result.GetProperty("id").ToString());
                Console.WriteLine(result.GetProperty("petId").ToString());
                Console.WriteLine(result.GetProperty("name").ToString());
            }
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetPets_ShortVersion_Async()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            await foreach (BinaryData item in client.GetPetsAsync(null))
            {
                JsonElement result = JsonDocument.Parse(item.ToStream()).RootElement;
                Console.WriteLine(result.GetProperty("id").ToString());
                Console.WriteLine(result.GetProperty("petId").ToString());
                Console.WriteLine(result.GetProperty("name").ToString());
            }
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetPets_AllParameters()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            foreach (BinaryData item in client.GetPets(null))
            {
                JsonElement result = JsonDocument.Parse(item.ToStream()).RootElement;
                Console.WriteLine(result.GetProperty("id").ToString());
                Console.WriteLine(result.GetProperty("petId").ToString());
                Console.WriteLine(result.GetProperty("name").ToString());
            }
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetPets_AllParameters_Async()
        {
            Uri endpoint = new Uri("<https://my-service.azure.com>");
            MixApiVersionClient client = new MixApiVersionClient(endpoint);

            await foreach (BinaryData item in client.GetPetsAsync(null))
            {
                JsonElement result = JsonDocument.Parse(item.ToStream()).RootElement;
                Console.WriteLine(result.GetProperty("id").ToString());
                Console.WriteLine(result.GetProperty("petId").ToString());
                Console.WriteLine(result.GetProperty("name").ToString());
            }
        }
    }
}
