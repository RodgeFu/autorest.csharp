// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;

namespace MgmtSafeFlatten
{
    /// <summary> A class to add extension methods to MgmtSafeFlatten. </summary>
    public static partial class MgmtSafeFlattenExtensions
    {
        private static SubscriptionResourceExtensionClient GetExtensionClient(SubscriptionResource subscriptionResource)
        {
            return subscriptionResource.GetCachedClient((client) =>
            {
                return new SubscriptionResourceExtensionClient(client, subscriptionResource.Id);
            }
            );
        }

        /// <summary>
        /// Description for Validate information for a certificate order.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.TypeOne/typeOnes</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Common_ListTypeOnesBySubscription</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="subscriptionResource"> The <see cref="SubscriptionResource" /> instance the method will execute against. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> An async collection of <see cref="TypeOneResource" /> that may take multiple service requests to iterate over. </returns>
        public static AsyncPageable<TypeOneResource> GetTypeOnesAsync(this SubscriptionResource subscriptionResource, CancellationToken cancellationToken = default)
        {
            return GetExtensionClient(subscriptionResource).GetTypeOnesAsync(cancellationToken);
        }

        /// <summary>
        /// Description for Validate information for a certificate order.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.TypeOne/typeOnes</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Common_ListTypeOnesBySubscription</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="subscriptionResource"> The <see cref="SubscriptionResource" /> instance the method will execute against. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> A collection of <see cref="TypeOneResource" /> that may take multiple service requests to iterate over. </returns>
        public static Pageable<TypeOneResource> GetTypeOnes(this SubscriptionResource subscriptionResource, CancellationToken cancellationToken = default)
        {
            return GetExtensionClient(subscriptionResource).GetTypeOnes(cancellationToken);
        }

        /// <summary>
        /// Description for Validate information for a certificate order.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.TypeTwo/typeTwos</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Common_ListTypeTwosBySubscription</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="subscriptionResource"> The <see cref="SubscriptionResource" /> instance the method will execute against. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> An async collection of <see cref="TypeTwoResource" /> that may take multiple service requests to iterate over. </returns>
        public static AsyncPageable<TypeTwoResource> GetTypeTwosAsync(this SubscriptionResource subscriptionResource, CancellationToken cancellationToken = default)
        {
            return GetExtensionClient(subscriptionResource).GetTypeTwosAsync(cancellationToken);
        }

        /// <summary>
        /// Description for Validate information for a certificate order.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.TypeTwo/typeTwos</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Common_ListTypeTwosBySubscription</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="subscriptionResource"> The <see cref="SubscriptionResource" /> instance the method will execute against. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <returns> A collection of <see cref="TypeTwoResource" /> that may take multiple service requests to iterate over. </returns>
        public static Pageable<TypeTwoResource> GetTypeTwos(this SubscriptionResource subscriptionResource, CancellationToken cancellationToken = default)
        {
            return GetExtensionClient(subscriptionResource).GetTypeTwos(cancellationToken);
        }

        private static ResourceGroupResourceExtensionClient GetExtensionClient(ResourceGroupResource resourceGroupResource)
        {
            return resourceGroupResource.GetCachedClient((client) =>
            {
                return new ResourceGroupResourceExtensionClient(client, resourceGroupResource.Id);
            }
            );
        }

        /// <summary> Gets a collection of TypeOneResources in the ResourceGroupResource. </summary>
        /// <param name="resourceGroupResource"> The <see cref="ResourceGroupResource" /> instance the method will execute against. </param>
        /// <returns> An object representing collection of TypeOneResources and their operations over a TypeOneResource. </returns>
        public static TypeOneCollection GetTypeOnes(this ResourceGroupResource resourceGroupResource)
        {
            return GetExtensionClient(resourceGroupResource).GetTypeOnes();
        }

        /// <summary>
        /// Description for Validate information for a certificate order.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.TypeOne/typeOnes/{typeOneName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Common_GetTypeOne</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="resourceGroupResource"> The <see cref="ResourceGroupResource" /> instance the method will execute against. </param>
        /// <param name="typeOneName"> The name. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="typeOneName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="typeOneName"/> is null. </exception>
        [ForwardsClientCalls]
        public static async Task<Response<TypeOneResource>> GetTypeOneAsync(this ResourceGroupResource resourceGroupResource, string typeOneName, CancellationToken cancellationToken = default)
        {
            return await resourceGroupResource.GetTypeOnes().GetAsync(typeOneName, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Description for Validate information for a certificate order.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.TypeOne/typeOnes/{typeOneName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Common_GetTypeOne</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="resourceGroupResource"> The <see cref="ResourceGroupResource" /> instance the method will execute against. </param>
        /// <param name="typeOneName"> The name. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="typeOneName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="typeOneName"/> is null. </exception>
        [ForwardsClientCalls]
        public static Response<TypeOneResource> GetTypeOne(this ResourceGroupResource resourceGroupResource, string typeOneName, CancellationToken cancellationToken = default)
        {
            return resourceGroupResource.GetTypeOnes().Get(typeOneName, cancellationToken);
        }

        /// <summary> Gets a collection of TypeTwoResources in the ResourceGroupResource. </summary>
        /// <param name="resourceGroupResource"> The <see cref="ResourceGroupResource" /> instance the method will execute against. </param>
        /// <returns> An object representing collection of TypeTwoResources and their operations over a TypeTwoResource. </returns>
        public static TypeTwoCollection GetTypeTwos(this ResourceGroupResource resourceGroupResource)
        {
            return GetExtensionClient(resourceGroupResource).GetTypeTwos();
        }

        /// <summary>
        /// Description for Validate information for a certificate order.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.TypeTwo/typeTwos/{typeTwoName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Common_GetTypeTwo</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="resourceGroupResource"> The <see cref="ResourceGroupResource" /> instance the method will execute against. </param>
        /// <param name="typeTwoName"> The name. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="typeTwoName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="typeTwoName"/> is null. </exception>
        [ForwardsClientCalls]
        public static async Task<Response<TypeTwoResource>> GetTypeTwoAsync(this ResourceGroupResource resourceGroupResource, string typeTwoName, CancellationToken cancellationToken = default)
        {
            return await resourceGroupResource.GetTypeTwos().GetAsync(typeTwoName, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Description for Validate information for a certificate order.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.TypeTwo/typeTwos/{typeTwoName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Common_GetTypeTwo</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="resourceGroupResource"> The <see cref="ResourceGroupResource" /> instance the method will execute against. </param>
        /// <param name="typeTwoName"> The name. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="typeTwoName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="typeTwoName"/> is null. </exception>
        [ForwardsClientCalls]
        public static Response<TypeTwoResource> GetTypeTwo(this ResourceGroupResource resourceGroupResource, string typeTwoName, CancellationToken cancellationToken = default)
        {
            return resourceGroupResource.GetTypeTwos().Get(typeTwoName, cancellationToken);
        }

        #region TypeOneResource
        /// <summary>
        /// Gets an object representing a <see cref="TypeOneResource" /> along with the instance operations that can be performed on it but with no data.
        /// You can use <see cref="TypeOneResource.CreateResourceIdentifier" /> to create a <see cref="TypeOneResource" /> <see cref="ResourceIdentifier" /> from its components.
        /// </summary>
        /// <param name="client"> The <see cref="ArmClient" /> instance the method will execute against. </param>
        /// <param name="id"> The resource ID of the resource to get. </param>
        /// <returns> Returns a <see cref="TypeOneResource" /> object. </returns>
        public static TypeOneResource GetTypeOneResource(this ArmClient client, ResourceIdentifier id)
        {
            return client.GetResourceClient(() =>
            {
                TypeOneResource.ValidateResourceId(id);
                return new TypeOneResource(client, id);
            }
            );
        }
        #endregion

        #region TypeTwoResource
        /// <summary>
        /// Gets an object representing a <see cref="TypeTwoResource" /> along with the instance operations that can be performed on it but with no data.
        /// You can use <see cref="TypeTwoResource.CreateResourceIdentifier" /> to create a <see cref="TypeTwoResource" /> <see cref="ResourceIdentifier" /> from its components.
        /// </summary>
        /// <param name="client"> The <see cref="ArmClient" /> instance the method will execute against. </param>
        /// <param name="id"> The resource ID of the resource to get. </param>
        /// <returns> Returns a <see cref="TypeTwoResource" /> object. </returns>
        public static TypeTwoResource GetTypeTwoResource(this ArmClient client, ResourceIdentifier id)
        {
            return client.GetResourceClient(() =>
            {
                TypeTwoResource.ValidateResourceId(id);
                return new TypeTwoResource(client, id);
            }
            );
        }
        #endregion
    }
}
