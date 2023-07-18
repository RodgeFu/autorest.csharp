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

namespace body_complex_LowLevel.Samples
{
    public class Samples_PolymorphicrecursiveClient
    {
        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetValid()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new PolymorphicrecursiveClient(credential);

            Response response = client.GetValid(new RequestContext());

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("fishtype").ToString());
            Console.WriteLine(result.GetProperty("length").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetValid_AllParameters()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new PolymorphicrecursiveClient(credential);

            Response response = client.GetValid(new RequestContext());

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("fishtype").ToString());
            Console.WriteLine(result.GetProperty("species").ToString());
            Console.WriteLine(result.GetProperty("length").ToString());
            Console.WriteLine(result.GetProperty("siblings")[0].GetProperty("fishtype").ToString());
            Console.WriteLine(result.GetProperty("siblings")[0].GetProperty("species").ToString());
            Console.WriteLine(result.GetProperty("siblings")[0].GetProperty("length").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetValid_Async()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new PolymorphicrecursiveClient(credential);

            Response response = await client.GetValidAsync(new RequestContext());

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("fishtype").ToString());
            Console.WriteLine(result.GetProperty("length").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetValid_AllParameters_Async()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new PolymorphicrecursiveClient(credential);

            Response response = await client.GetValidAsync(new RequestContext());

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("fishtype").ToString());
            Console.WriteLine(result.GetProperty("species").ToString());
            Console.WriteLine(result.GetProperty("length").ToString());
            Console.WriteLine(result.GetProperty("siblings")[0].GetProperty("fishtype").ToString());
            Console.WriteLine(result.GetProperty("siblings")[0].GetProperty("species").ToString());
            Console.WriteLine(result.GetProperty("siblings")[0].GetProperty("length").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_PutValid()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new PolymorphicrecursiveClient(credential);

            var data = new
            {
                fishtype = "salmon",
                length = 123.45f,
            };

            Response response = client.PutValid(RequestContent.Create(data));
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_PutValid_AllParameters()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new PolymorphicrecursiveClient(credential);

            var data = new
            {
                location = "<location>",
                iswild = true,
                fishtype = "salmon",
                species = "<species>",
                length = 123.45f,
            };

            Response response = client.PutValid(RequestContent.Create(data));
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_PutValid_Async()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new PolymorphicrecursiveClient(credential);

            var data = new
            {
                fishtype = "salmon",
                length = 123.45f,
            };

            Response response = await client.PutValidAsync(RequestContent.Create(data));
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_PutValid_AllParameters_Async()
        {
            var credential = new AzureKeyCredential("<key>");
            var client = new PolymorphicrecursiveClient(credential);

            var data = new
            {
                location = "<location>",
                iswild = true,
                fishtype = "salmon",
                species = "<species>",
                length = 123.45f,
            };

            Response response = await client.PutValidAsync(RequestContent.Create(data));
            Console.WriteLine(response.Status);
        }
    }
}
