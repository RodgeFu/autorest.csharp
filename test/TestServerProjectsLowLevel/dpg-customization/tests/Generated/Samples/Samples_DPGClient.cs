// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Identity;
using NUnit.Framework;

namespace dpg_customization_LowLevel.Samples
{
    public class Samples_DPGClient
    {
        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetModel()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            Response response = client.GetModel("<mode>");

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("received").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetModel_AllParameters()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            Response response = client.GetModel("<mode>", new RequestContext());

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("received").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetModel_Async()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            Response response = await client.GetModelAsync("<mode>");

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("received").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetModel_AllParameters_Async()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            Response response = await client.GetModelAsync("<mode>", new RequestContext());

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("received").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_PostModel()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            var data = new
            {
                hello = "<hello>",
            };

            Response response = client.PostModel("<mode>", RequestContent.Create(data));

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("received").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_PostModel_AllParameters()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            var data = new
            {
                hello = "<hello>",
            };

            Response response = client.PostModel("<mode>", RequestContent.Create(data), new RequestContext());

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("received").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_PostModel_Async()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            var data = new
            {
                hello = "<hello>",
            };

            Response response = await client.PostModelAsync("<mode>", RequestContent.Create(data));

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("received").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_PostModel_AllParameters_Async()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            var data = new
            {
                hello = "<hello>",
            };

            Response response = await client.PostModelAsync("<mode>", RequestContent.Create(data), new RequestContext());

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("received").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetPages()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            foreach (var item in client.GetPages("<mode>"))
            {
                JsonElement result = JsonDocument.Parse(item.ToStream()).RootElement;
                Console.WriteLine(result.GetProperty("received").ToString());
            }
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetPages_AllParameters()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            foreach (var item in client.GetPages("<mode>", new RequestContext()))
            {
                JsonElement result = JsonDocument.Parse(item.ToStream()).RootElement;
                Console.WriteLine(result.GetProperty("received").ToString());
            }
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetPages_Async()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            await foreach (var item in client.GetPagesAsync("<mode>"))
            {
                JsonElement result = JsonDocument.Parse(item.ToStream()).RootElement;
                Console.WriteLine(result.GetProperty("received").ToString());
            }
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetPages_AllParameters_Async()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            await foreach (var item in client.GetPagesAsync("<mode>", new RequestContext()))
            {
                JsonElement result = JsonDocument.Parse(item.ToStream()).RootElement;
                Console.WriteLine(result.GetProperty("received").ToString());
            }
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_Lro()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            var operation = client.Lro(WaitUntil.Completed, "<mode>");

            BinaryData responseData = operation.Value;
            JsonElement result = JsonDocument.Parse(responseData.ToStream()).RootElement;
            Console.WriteLine(result.GetProperty("provisioningState").ToString());
            Console.WriteLine(result.GetProperty("received").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_Lro_AllParameters()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            var operation = client.Lro(WaitUntil.Completed, "<mode>", new RequestContext());

            BinaryData responseData = operation.Value;
            JsonElement result = JsonDocument.Parse(responseData.ToStream()).RootElement;
            Console.WriteLine(result.GetProperty("provisioningState").ToString());
            Console.WriteLine(result.GetProperty("received").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_Lro_Async()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            var operation = await client.LroAsync(WaitUntil.Completed, "<mode>");

            BinaryData responseData = operation.Value;
            JsonElement result = JsonDocument.Parse(responseData.ToStream()).RootElement;
            Console.WriteLine(result.GetProperty("provisioningState").ToString());
            Console.WriteLine(result.GetProperty("received").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_Lro_AllParameters_Async()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new DPGClient(credential);

            var operation = await client.LroAsync(WaitUntil.Completed, "<mode>", new RequestContext());

            BinaryData responseData = operation.Value;
            JsonElement result = JsonDocument.Parse(responseData.ToStream()).RootElement;
            Console.WriteLine(result.GetProperty("provisioningState").ToString());
            Console.WriteLine(result.GetProperty("received").ToString());
        }
    }
}
