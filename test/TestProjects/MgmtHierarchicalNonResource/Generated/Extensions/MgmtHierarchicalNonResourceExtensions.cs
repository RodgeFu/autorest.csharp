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

namespace MgmtHierarchicalNonResource
{
    /// <summary> A class to add extension methods to MgmtHierarchicalNonResource. </summary>
    public static partial class MgmtHierarchicalNonResourceExtensions
    {
        private static SubscriptionResourceExtensionClient GetSubscriptionResourceExtensionClient(ArmResource resource)
        {
            return resource.GetCachedClient(client =>
            {
                return new SubscriptionResourceExtensionClient(client, resource.Id);
            });
        }

        private static SubscriptionResourceExtensionClient GetSubscriptionResourceExtensionClient(ArmClient client, ResourceIdentifier scope)
        {
            return client.GetResourceClient(() =>
            {
                return new SubscriptionResourceExtensionClient(client, scope);
            });
        }
        #region SharedGalleryResource
        /// <summary>
        /// Gets an object representing a <see cref="SharedGalleryResource" /> along with the instance operations that can be performed on it but with no data.
        /// You can use <see cref="SharedGalleryResource.CreateResourceIdentifier" /> to create a <see cref="SharedGalleryResource" /> <see cref="ResourceIdentifier" /> from its components.
        /// </summary>
        /// <param name="client"> The <see cref="ArmClient" /> instance the method will execute against. </param>
        /// <param name="id"> The resource ID of the resource to get. </param>
        /// <returns> Returns a <see cref="SharedGalleryResource" /> object. </returns>
        public static SharedGalleryResource GetSharedGalleryResource(this ArmClient client, ResourceIdentifier id)
        {
            return client.GetResourceClient(() =>
            {
                SharedGalleryResource.ValidateResourceId(id);
                return new SharedGalleryResource(client, id);
            }
            );
        }
        #endregion

        /// <summary> Gets a collection of SharedGalleryResources in the SubscriptionResource. </summary>
        /// <param name="subscriptionResource"> The <see cref="SubscriptionResource" /> instance the method will execute against. </param>
        /// <param name="location"> Resource location. </param>
        /// <exception cref="ArgumentException"> <paramref name="location"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="location"/> is null. </exception>
        /// <returns> An object representing collection of SharedGalleryResources and their operations over a SharedGalleryResource. </returns>
        public static SharedGalleryCollection GetSharedGalleries(this SubscriptionResource subscriptionResource, string location)
        {
            Argument.AssertNotNullOrEmpty(location, nameof(location));

            return GetSubscriptionResourceExtensionClient(subscriptionResource).GetSharedGalleries(location);
        }

        /// <summary>
        /// Get a shared gallery by subscription id or tenant id.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.Compute/locations/{location}/sharedGalleries/{galleryUniqueName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SharedGalleries_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="subscriptionResource"> The <see cref="SubscriptionResource" /> instance the method will execute against. </param>
        /// <param name="location"> Resource location. </param>
        /// <param name="galleryUniqueName"> The unique name of the Shared Gallery. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="location"/> or <paramref name="galleryUniqueName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="location"/> or <paramref name="galleryUniqueName"/> is null. </exception>
        [ForwardsClientCalls]
        public static async Task<Response<SharedGalleryResource>> GetSharedGalleryAsync(this SubscriptionResource subscriptionResource, string location, string galleryUniqueName, CancellationToken cancellationToken = default)
        {
            return await subscriptionResource.GetSharedGalleries(location).GetAsync(galleryUniqueName, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a shared gallery by subscription id or tenant id.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.Compute/locations/{location}/sharedGalleries/{galleryUniqueName}</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>SharedGalleries_Get</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="subscriptionResource"> The <see cref="SubscriptionResource" /> instance the method will execute against. </param>
        /// <param name="location"> Resource location. </param>
        /// <param name="galleryUniqueName"> The unique name of the Shared Gallery. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentException"> <paramref name="location"/> or <paramref name="galleryUniqueName"/> is an empty string, and was expected to be non-empty. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="location"/> or <paramref name="galleryUniqueName"/> is null. </exception>
        [ForwardsClientCalls]
        public static Response<SharedGalleryResource> GetSharedGallery(this SubscriptionResource subscriptionResource, string location, string galleryUniqueName, CancellationToken cancellationToken = default)
        {
            return subscriptionResource.GetSharedGalleries(location).Get(galleryUniqueName, cancellationToken);
        }
    }
}
