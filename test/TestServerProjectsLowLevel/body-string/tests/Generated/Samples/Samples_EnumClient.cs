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
using NUnit.Framework;
using body_string_LowLevel;

namespace body_string_LowLevel.Samples
{
    public class Samples_EnumClient
    {
        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetNotExpandable()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            Response response = client.GetNotExpandable(null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetNotExpandable_Async()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            Response response = await client.GetNotExpandableAsync(null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetNotExpandable_AllParameters()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            Response response = client.GetNotExpandable(null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetNotExpandable_AllParameters_Async()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            Response response = await client.GetNotExpandableAsync(null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_PutNotExpandable()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            RequestContent content = RequestContent.Create("red color");
            Response response = client.PutNotExpandable(content);
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_PutNotExpandable_Async()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            RequestContent content = RequestContent.Create("red color");
            Response response = await client.PutNotExpandableAsync(content);
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_PutNotExpandable_AllParameters()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            RequestContent content = RequestContent.Create("red color");
            Response response = client.PutNotExpandable(content);
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_PutNotExpandable_AllParameters_Async()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            RequestContent content = RequestContent.Create("red color");
            Response response = await client.PutNotExpandableAsync(content);
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetReferenced()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            Response response = client.GetReferenced(null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetReferenced_Async()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            Response response = await client.GetReferencedAsync(null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetReferenced_AllParameters()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            Response response = client.GetReferenced(null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetReferenced_AllParameters_Async()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            Response response = await client.GetReferencedAsync(null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_PutReferenced()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            RequestContent content = RequestContent.Create("red color");
            Response response = client.PutReferenced(content);
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_PutReferenced_Async()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            RequestContent content = RequestContent.Create("red color");
            Response response = await client.PutReferencedAsync(content);
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_PutReferenced_AllParameters()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            RequestContent content = RequestContent.Create("red color");
            Response response = client.PutReferenced(content);
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_PutReferenced_AllParameters_Async()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            RequestContent content = RequestContent.Create("red color");
            Response response = await client.PutReferencedAsync(content);
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetReferencedConstant()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            Response response = client.GetReferencedConstant(null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("ColorConstant").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetReferencedConstant_Async()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            Response response = await client.GetReferencedConstantAsync(null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("ColorConstant").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_GetReferencedConstant_AllParameters()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            Response response = client.GetReferencedConstant(null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("ColorConstant").ToString());
            Console.WriteLine(result.GetProperty("field1").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_GetReferencedConstant_AllParameters_Async()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            Response response = await client.GetReferencedConstantAsync(null);

            JsonElement result = JsonDocument.Parse(response.ContentStream).RootElement;
            Console.WriteLine(result.GetProperty("ColorConstant").ToString());
            Console.WriteLine(result.GetProperty("field1").ToString());
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_PutReferencedConstant()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            RequestContent content = RequestContent.Create(new
            {
                ColorConstant = "green-color",
            });
            Response response = client.PutReferencedConstant(content);
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_PutReferencedConstant_Async()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            RequestContent content = RequestContent.Create(new
            {
                ColorConstant = "green-color",
            });
            Response response = await client.PutReferencedConstantAsync(content);
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public void Example_PutReferencedConstant_AllParameters()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            RequestContent content = RequestContent.Create(new
            {
                ColorConstant = "green-color",
                field1 = "<field1>",
            });
            Response response = client.PutReferencedConstant(content);
            Console.WriteLine(response.Status);
        }

        [Test]
        [Ignore("Only validating compilation of examples")]
        public async Task Example_PutReferencedConstant_AllParameters_Async()
        {
            AzureKeyCredential credential = new AzureKeyCredential("<key>");
            EnumClient client = new EnumClient(credential);

            RequestContent content = RequestContent.Create(new
            {
                ColorConstant = "green-color",
                field1 = "<field1>",
            });
            Response response = await client.PutReferencedConstantAsync(content);
            Console.WriteLine(response.Status);
        }
    }
}
