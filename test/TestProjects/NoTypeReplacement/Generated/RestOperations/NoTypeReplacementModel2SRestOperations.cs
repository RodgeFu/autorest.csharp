// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.ResourceManager;
using Azure.ResourceManager.Core;
using NoTypeReplacement.Models;

namespace NoTypeReplacement
{
    internal partial class NoTypeReplacementModel2SRestOperations
    {
        private Uri endpoint;
        private string apiVersion;
        private ClientDiagnostics _clientDiagnostics;
        private HttpPipeline _pipeline;
        private readonly string _userAgent;

        /// <summary> Initializes a new instance of NoTypeReplacementModel2SRestOperations. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="options"> The client options used to construct the current client. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <param name="apiVersion"> Api Version. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="apiVersion"/> is null. </exception>
        public NoTypeReplacementModel2SRestOperations(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, ArmClientOptions options, Uri endpoint = null, string apiVersion = default)
        {
            this.endpoint = endpoint ?? new Uri("https://management.azure.com");
            this.apiVersion = apiVersion ?? "2020-06-01";
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
            _userAgent = HttpMessageUtilities.GetUserAgentName(this, options);
        }

        internal HttpMessage CreateListRequest(string subscriptionId, string resourceGroupName)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/subscriptions/", false);
            uri.AppendPath(subscriptionId, true);
            uri.AppendPath("/resourceGroups/", false);
            uri.AppendPath(resourceGroupName, true);
            uri.AppendPath("/providers/Microsoft.Compute/noTypeReplacementModel2s", false);
            uri.AppendQuery("api-version", apiVersion, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            message.SetProperty("UserAgentOverride", _userAgent);
            return message;
        }

        /// <param name="subscriptionId"> The String to use. </param>
        /// <param name="resourceGroupName"> The name of the resource group. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="subscriptionId"/> or <paramref name="resourceGroupName"/> is null. </exception>
        public async Task<Response<NoTypeReplacementModel2ListResult>> ListAsync(string subscriptionId, string resourceGroupName, CancellationToken cancellationToken = default)
        {
            if (subscriptionId == null)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }
            if (resourceGroupName == null)
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }

            using var message = CreateListRequest(subscriptionId, resourceGroupName);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        NoTypeReplacementModel2ListResult value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = NoTypeReplacementModel2ListResult.DeserializeNoTypeReplacementModel2ListResult(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <param name="subscriptionId"> The String to use. </param>
        /// <param name="resourceGroupName"> The name of the resource group. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="subscriptionId"/> or <paramref name="resourceGroupName"/> is null. </exception>
        public Response<NoTypeReplacementModel2ListResult> List(string subscriptionId, string resourceGroupName, CancellationToken cancellationToken = default)
        {
            if (subscriptionId == null)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }
            if (resourceGroupName == null)
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }

            using var message = CreateListRequest(subscriptionId, resourceGroupName);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        NoTypeReplacementModel2ListResult value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = NoTypeReplacementModel2ListResult.DeserializeNoTypeReplacementModel2ListResult(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreatePutRequest(string subscriptionId, string resourceGroupName, string noTypeReplacementModel2SName, NoTypeReplacementModel2Data parameters)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Put;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/subscriptions/", false);
            uri.AppendPath(subscriptionId, true);
            uri.AppendPath("/resourceGroups/", false);
            uri.AppendPath(resourceGroupName, true);
            uri.AppendPath("/providers/Microsoft.Compute/noTypeReplacementModel2s/", false);
            uri.AppendPath(noTypeReplacementModel2SName, true);
            uri.AppendQuery("api-version", apiVersion, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Content-Type", "application/json");
            var content = new Utf8JsonRequestContent();
            content.JsonWriter.WriteObjectValue(parameters);
            request.Content = content;
            message.SetProperty("UserAgentOverride", _userAgent);
            return message;
        }

        /// <param name="subscriptionId"> The String to use. </param>
        /// <param name="resourceGroupName"> The String to use. </param>
        /// <param name="noTypeReplacementModel2SName"> The String to use. </param>
        /// <param name="parameters"> The NoTypeReplacementModel2 to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="noTypeReplacementModel2SName"/>, or <paramref name="parameters"/> is null. </exception>
        public async Task<Response<NoTypeReplacementModel2Data>> PutAsync(string subscriptionId, string resourceGroupName, string noTypeReplacementModel2SName, NoTypeReplacementModel2Data parameters, CancellationToken cancellationToken = default)
        {
            if (subscriptionId == null)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }
            if (resourceGroupName == null)
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }
            if (noTypeReplacementModel2SName == null)
            {
                throw new ArgumentNullException(nameof(noTypeReplacementModel2SName));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using var message = CreatePutRequest(subscriptionId, resourceGroupName, noTypeReplacementModel2SName, parameters);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        NoTypeReplacementModel2Data value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = NoTypeReplacementModel2Data.DeserializeNoTypeReplacementModel2Data(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <param name="subscriptionId"> The String to use. </param>
        /// <param name="resourceGroupName"> The String to use. </param>
        /// <param name="noTypeReplacementModel2SName"> The String to use. </param>
        /// <param name="parameters"> The NoTypeReplacementModel2 to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, <paramref name="noTypeReplacementModel2SName"/>, or <paramref name="parameters"/> is null. </exception>
        public Response<NoTypeReplacementModel2Data> Put(string subscriptionId, string resourceGroupName, string noTypeReplacementModel2SName, NoTypeReplacementModel2Data parameters, CancellationToken cancellationToken = default)
        {
            if (subscriptionId == null)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }
            if (resourceGroupName == null)
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }
            if (noTypeReplacementModel2SName == null)
            {
                throw new ArgumentNullException(nameof(noTypeReplacementModel2SName));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using var message = CreatePutRequest(subscriptionId, resourceGroupName, noTypeReplacementModel2SName, parameters);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        NoTypeReplacementModel2Data value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = NoTypeReplacementModel2Data.DeserializeNoTypeReplacementModel2Data(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateGetRequest(string subscriptionId, string resourceGroupName, string noTypeReplacementModel2SName)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/subscriptions/", false);
            uri.AppendPath(subscriptionId, true);
            uri.AppendPath("/resourceGroups/", false);
            uri.AppendPath(resourceGroupName, true);
            uri.AppendPath("/providers/Microsoft.Compute/noTypeReplacementModel2s/", false);
            uri.AppendPath(noTypeReplacementModel2SName, true);
            uri.AppendQuery("api-version", apiVersion, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/json");
            message.SetProperty("UserAgentOverride", _userAgent);
            return message;
        }

        /// <param name="subscriptionId"> The String to use. </param>
        /// <param name="resourceGroupName"> The name of the resource group. </param>
        /// <param name="noTypeReplacementModel2SName"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, or <paramref name="noTypeReplacementModel2SName"/> is null. </exception>
        public async Task<Response<NoTypeReplacementModel2Data>> GetAsync(string subscriptionId, string resourceGroupName, string noTypeReplacementModel2SName, CancellationToken cancellationToken = default)
        {
            if (subscriptionId == null)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }
            if (resourceGroupName == null)
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }
            if (noTypeReplacementModel2SName == null)
            {
                throw new ArgumentNullException(nameof(noTypeReplacementModel2SName));
            }

            using var message = CreateGetRequest(subscriptionId, resourceGroupName, noTypeReplacementModel2SName);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        NoTypeReplacementModel2Data value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = NoTypeReplacementModel2Data.DeserializeNoTypeReplacementModel2Data(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 404:
                    return Response.FromValue((NoTypeReplacementModel2Data)null, message.Response);
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <param name="subscriptionId"> The String to use. </param>
        /// <param name="resourceGroupName"> The name of the resource group. </param>
        /// <param name="noTypeReplacementModel2SName"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="subscriptionId"/>, <paramref name="resourceGroupName"/>, or <paramref name="noTypeReplacementModel2SName"/> is null. </exception>
        public Response<NoTypeReplacementModel2Data> Get(string subscriptionId, string resourceGroupName, string noTypeReplacementModel2SName, CancellationToken cancellationToken = default)
        {
            if (subscriptionId == null)
            {
                throw new ArgumentNullException(nameof(subscriptionId));
            }
            if (resourceGroupName == null)
            {
                throw new ArgumentNullException(nameof(resourceGroupName));
            }
            if (noTypeReplacementModel2SName == null)
            {
                throw new ArgumentNullException(nameof(noTypeReplacementModel2SName));
            }

            using var message = CreateGetRequest(subscriptionId, resourceGroupName, noTypeReplacementModel2SName);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        NoTypeReplacementModel2Data value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = NoTypeReplacementModel2Data.DeserializeNoTypeReplacementModel2Data(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 404:
                    return Response.FromValue((NoTypeReplacementModel2Data)null, message.Response);
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }
    }
}
