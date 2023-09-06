// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Autorest.CSharp.Core;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

namespace Azure.ResourceManager.Sample
{
    /// <summary>
    /// A class representing a collection of <see cref="SshPublicKeyResource" /> and their operations.
    /// Each <see cref="SshPublicKeyResource" /> in the collection will belong to the same instance of <see cref="ResourceGroupResource" />.
    /// To get a <see cref="SshPublicKeyCollection" /> instance call the GetSshPublicKeys method from an instance of <see cref="ResourceGroupResource" />.
    /// </summary>
    public partial class SshPublicKeyCollection : ArmCollection, IEnumerable<SshPublicKeyResource>, IAsyncEnumerable<SshPublicKeyResource>
    {
        private readonly ClientDiagnostics _sshPublicKeyClientDiagnostics;
        private readonly SshPublicKeysRestOperations _sshPublicKeyRestClient;

        /// <summary> Initializes a new instance of the <see cref="SshPublicKeyCollection"/> class for mocking. </summary>
        protected SshPublicKeyCollection()
        {
        }

        /// <summary> Initializes a new instance of the <see cref="SshPublicKeyCollection"/> class. </summary>
        /// <param name="client"> The client parameters to use in these operations. </param>
        /// <param name="id"> The identifier of the parent resource that is the target of operations. </param>
        internal SshPublicKeyCollection(ArmClient client, ResourceIdentifier id) : base(client, id)
        {
            _sshPublicKeyClientDiagnostics = new ClientDiagnostics("Azure.ResourceManager.Sample", SshPublicKeyResource.ResourceType.Namespace, Diagnostics);
            TryGetApiVersion(SshPublicKeyResource.ResourceType, out string sshPublicKeyApiVersion);
            _sshPublicKeyRestClient = new SshPublicKeysRestOperations(Pipeline, Diagnostics.ApplicationId, Endpoint, sshPublicKeyApiVersion);
#if DEBUG
			ValidateResourceId(Id);
#endif
        }

        internal static void ValidateResourceId(ResourceIdentifier id)
        {
            if (id.ResourceType != ResourceGroupResource.ResourceType)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Invalid resource type {0} expected {1}", id.ResourceType, ResourceGroupResource.ResourceType), nameof(id));
        }

        /// <summary>
        /// Creates a new SSH public key resource.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Compute/sshPublicKeys/{sshPublicKeyName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SshPublicKeys_Create</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="waitUntil"> <see cref="WaitUntil.Completed"/> if the method should wait to return until the long-running operation has completed on the service; <see cref="WaitUntil.Started"/> if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="sshPublicKeyName"> The name of the SSH public key. </param>
        /// <param name="data"> Parameters supplied to create the SSH public key. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sshPublicKeyName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sshPublicKeyName"/> or <paramref name="data"/> is null. </exception>
        public virtual async Task<ArmOperation<SshPublicKeyResource>> CreateOrUpdateAsync(WaitUntil waitUntil, string sshPublicKeyName, SshPublicKeyData data, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sshPublicKeyName, nameof(sshPublicKeyName));
            Argument.AssertNotNull(data, nameof(data));

            using var scope = _sshPublicKeyClientDiagnostics.CreateScope("SshPublicKeyCollection.CreateOrUpdate");
            scope.Start();
            try
            {
                var response = await _sshPublicKeyRestClient.CreateAsync(Id.SubscriptionId, Id.ResourceGroupName, sshPublicKeyName, data, cancellationToken).ConfigureAwait(false);
                var operation = new SampleArmOperation<SshPublicKeyResource>(Response.FromValue(new SshPublicKeyResource(Client, response), response.GetRawResponse()));
                if (waitUntil == WaitUntil.Completed)
                    await operation.WaitForCompletionAsync(cancellationToken).ConfigureAwait(false);
                return operation;
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Creates a new SSH public key resource.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Compute/sshPublicKeys/{sshPublicKeyName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SshPublicKeys_Create</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="waitUntil"> <see cref="WaitUntil.Completed"/> if the method should wait to return until the long-running operation has completed on the service; <see cref="WaitUntil.Started"/> if it should return after starting the operation. For more information on long-running operations, please see <see href="https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/core/Azure.Core/samples/LongRunningOperations.md"> Azure.Core Long-Running Operation samples</see>. </param>
        /// <param name="sshPublicKeyName"> The name of the SSH public key. </param>
        /// <param name="data"> Parameters supplied to create the SSH public key. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sshPublicKeyName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sshPublicKeyName"/> or <paramref name="data"/> is null. </exception>
        public virtual ArmOperation<SshPublicKeyResource> CreateOrUpdate(WaitUntil waitUntil, string sshPublicKeyName, SshPublicKeyData data, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sshPublicKeyName, nameof(sshPublicKeyName));
            Argument.AssertNotNull(data, nameof(data));

            using var scope = _sshPublicKeyClientDiagnostics.CreateScope("SshPublicKeyCollection.CreateOrUpdate");
            scope.Start();
            try
            {
                var response = _sshPublicKeyRestClient.Create(Id.SubscriptionId, Id.ResourceGroupName, sshPublicKeyName, data, cancellationToken);
                var operation = new SampleArmOperation<SshPublicKeyResource>(Response.FromValue(new SshPublicKeyResource(Client, response), response.GetRawResponse()));
                if (waitUntil == WaitUntil.Completed)
                    operation.WaitForCompletion(cancellationToken);
                return operation;
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Retrieves information about an SSH public key.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Compute/sshPublicKeys/{sshPublicKeyName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SshPublicKeys_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="sshPublicKeyName"> The name of the SSH public key. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sshPublicKeyName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sshPublicKeyName"/> is null. </exception>
        public virtual async Task<Response<SshPublicKeyResource>> GetAsync(string sshPublicKeyName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sshPublicKeyName, nameof(sshPublicKeyName));

            using var scope = _sshPublicKeyClientDiagnostics.CreateScope("SshPublicKeyCollection.Get");
            scope.Start();
            try
            {
                var response = await _sshPublicKeyRestClient.GetAsync(Id.SubscriptionId, Id.ResourceGroupName, sshPublicKeyName, cancellationToken).ConfigureAwait(false);
                if (response.Value == null)
                    throw new RequestFailedException(response.GetRawResponse());
                return Response.FromValue(new SshPublicKeyResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Retrieves information about an SSH public key.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Compute/sshPublicKeys/{sshPublicKeyName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SshPublicKeys_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="sshPublicKeyName"> The name of the SSH public key. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sshPublicKeyName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sshPublicKeyName"/> is null. </exception>
        public virtual Response<SshPublicKeyResource> Get(string sshPublicKeyName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sshPublicKeyName, nameof(sshPublicKeyName));

            using var scope = _sshPublicKeyClientDiagnostics.CreateScope("SshPublicKeyCollection.Get");
            scope.Start();
            try
            {
                var response = _sshPublicKeyRestClient.Get(Id.SubscriptionId, Id.ResourceGroupName, sshPublicKeyName, cancellationToken);
                if (response.Value == null)
                    throw new RequestFailedException(response.GetRawResponse());
                return Response.FromValue(new SshPublicKeyResource(Client, response.Value), response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Lists all of the SSH public keys in the specified resource group. Use the nextLink property in the response to get the next page of SSH public keys.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Compute/sshPublicKeys</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SshPublicKeys_ListByResourceGroup</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> An async collection of <see cref="SshPublicKeyResource" /> that may take multiple service requests to iterate over. </returns>
        public virtual AsyncPageable<SshPublicKeyResource> GetAllAsync(CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => _sshPublicKeyRestClient.CreateListByResourceGroupRequest(Id.SubscriptionId, Id.ResourceGroupName);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => _sshPublicKeyRestClient.CreateListByResourceGroupNextPageRequest(nextLink, Id.SubscriptionId, Id.ResourceGroupName);
            return GeneratorPageableHelpers.CreateAsyncPageable(FirstPageRequest, NextPageRequest, e => new SshPublicKeyResource(Client, SshPublicKeyData.DeserializeSshPublicKeyData(e)), _sshPublicKeyClientDiagnostics, Pipeline, "SshPublicKeyCollection.GetAll", "value", "nextLink", cancellationToken);
        }

        /// <summary>
        /// Lists all of the SSH public keys in the specified resource group. Use the nextLink property in the response to get the next page of SSH public keys.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Compute/sshPublicKeys</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SshPublicKeys_ListByResourceGroup</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> A collection of <see cref="SshPublicKeyResource" /> that may take multiple service requests to iterate over. </returns>
        public virtual Pageable<SshPublicKeyResource> GetAll(CancellationToken cancellationToken = default)
        {
            HttpMessage FirstPageRequest(int? pageSizeHint) => _sshPublicKeyRestClient.CreateListByResourceGroupRequest(Id.SubscriptionId, Id.ResourceGroupName);
            HttpMessage NextPageRequest(int? pageSizeHint, string nextLink) => _sshPublicKeyRestClient.CreateListByResourceGroupNextPageRequest(nextLink, Id.SubscriptionId, Id.ResourceGroupName);
            return GeneratorPageableHelpers.CreatePageable(FirstPageRequest, NextPageRequest, e => new SshPublicKeyResource(Client, SshPublicKeyData.DeserializeSshPublicKeyData(e)), _sshPublicKeyClientDiagnostics, Pipeline, "SshPublicKeyCollection.GetAll", "value", "nextLink", cancellationToken);
        }

        /// <summary>
        /// Checks to see if the resource exists in azure.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Compute/sshPublicKeys/{sshPublicKeyName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SshPublicKeys_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="sshPublicKeyName"> The name of the SSH public key. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sshPublicKeyName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sshPublicKeyName"/> is null. </exception>
        public virtual async Task<Response<bool>> ExistsAsync(string sshPublicKeyName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sshPublicKeyName, nameof(sshPublicKeyName));

            using var scope = _sshPublicKeyClientDiagnostics.CreateScope("SshPublicKeyCollection.Exists");
            scope.Start();
            try
            {
                var response = await _sshPublicKeyRestClient.GetAsync(Id.SubscriptionId, Id.ResourceGroupName, sshPublicKeyName, cancellationToken: cancellationToken).ConfigureAwait(false);
                return Response.FromValue(response.Value != null, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary>
        /// Checks to see if the resource exists in azure.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.Compute/sshPublicKeys/{sshPublicKeyName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SshPublicKeys_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="sshPublicKeyName"> The name of the SSH public key. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="sshPublicKeyName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="sshPublicKeyName"/> is null. </exception>
        public virtual Response<bool> Exists(string sshPublicKeyName, CancellationToken cancellationToken = default)
        {
            Argument.AssertNotNullOrEmpty(sshPublicKeyName, nameof(sshPublicKeyName));

            using var scope = _sshPublicKeyClientDiagnostics.CreateScope("SshPublicKeyCollection.Exists");
            scope.Start();
            try
            {
                var response = _sshPublicKeyRestClient.Get(Id.SubscriptionId, Id.ResourceGroupName, sshPublicKeyName, cancellationToken: cancellationToken);
                return Response.FromValue(response.Value != null, response.GetRawResponse());
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        IEnumerator<SshPublicKeyResource> IEnumerable<SshPublicKeyResource>.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        IAsyncEnumerator<SshPublicKeyResource> IAsyncEnumerable<SshPublicKeyResource>.GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            return GetAllAsync(cancellationToken: cancellationToken).GetAsyncEnumerator(cancellationToken);
        }
    }
}
