// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRest.SdkExplorer.Model.Azure
{
    internal class AzureResourceIdentifier
    {
        internal struct AzureResourceIdentifierSegment
        {
            public AzureResourceIdentifierSegment(string segName, string? segValue)
            {
                this.SegName = segName;
                this.SegValue = segValue;
            }
            public string SegName { get; set; }
            public string? SegValue { get; set; }

            public override string ToString()
            {
                return SegName + (string.IsNullOrEmpty(SegValue) ? "" : $"/{SegValue}");
            }
        }
        public string RawId { get; private set; }
        public string Id
        {
            get
            {
                string resourcePart = "/" + string.Join("/", this.ResourceSegments.Select(s => s.ToString()));
                string actionPart = this.ActionSegment == null ? "" : $"/{this.ActionSegment}";
                return $"{resourcePart}{actionPart}";
            }
        }
        public IReadOnlyList<AzureResourceIdentifierSegment> ResourceSegments { get; private set; } = new List<AzureResourceIdentifierSegment>();
        public AzureResourceIdentifierSegment? ActionSegment { get; private set; }

        public string? SubscriptionId { get; private set; }
        public string? ResourceGroupName { get; private set; }
        public AzureResourceType? ResourceType { get; private set; }

        private AzureResourceIdentifier(string rawId)
        {
            this.RawId = rawId;
        }

        public static List<AzureResourceIdentifierSegment> CreateSegmentsList(string[] arr, out AzureResourceIdentifierSegment? action)
        {
            int segLen = arr.Length / 2;
            List<AzureResourceIdentifierSegment> segs = new List<AzureResourceIdentifierSegment>();
            for (int i = 0; i < segLen; i++)
            {
                segs.Add(new AzureResourceIdentifierSegment(arr[2 * i], arr[2 * i + 1]));
            }
            action = (arr.Length % 2 != 0) ? new AzureResourceIdentifierSegment(arr[^1], null) : null;
            return segs;
        }

        public static AzureResourceIdentifier Parse(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            string[] arr = id.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (arr.Length == 0)
                throw new ArgumentException("At least one segment is expected in id. Id=" + id);

            var segList = CreateSegmentsList(arr, out AzureResourceIdentifierSegment? action);
            AzureResourceIdentifier r = new AzureResourceIdentifier(id);
            r.ResourceSegments = segList;
            r.ActionSegment = action;

            const int INDEX_SUBSCRIPTIONS = 0;
            const int INDEX_RESOURCEGROUP = 1;
            const int INDEX_PROVIDERS = 2;
            const int INDEX_TOP_TYPE = 3;
            if (segList.Count > INDEX_SUBSCRIPTIONS && segList[INDEX_SUBSCRIPTIONS].SegName.Equals("subscriptions", StringComparison.InvariantCultureIgnoreCase))
                r.SubscriptionId = segList[INDEX_SUBSCRIPTIONS].SegValue;
            if (segList.Count > INDEX_RESOURCEGROUP && segList[INDEX_RESOURCEGROUP].SegName.Equals("resourcegroups", StringComparison.InvariantCultureIgnoreCase))
                r.ResourceGroupName = segList[INDEX_RESOURCEGROUP].SegValue;
            string? ns = null;
            if (segList.Count > INDEX_PROVIDERS && segList[INDEX_PROVIDERS].SegName.Equals("providers", StringComparison.InvariantCultureIgnoreCase))
                ns = segList[INDEX_PROVIDERS].SegValue;
            if (!string.IsNullOrEmpty(ns))
            {
                string resourceType = string.Join("/", segList.Take(new Range(INDEX_TOP_TYPE, segList.Count)).Select(s => s.SegName));
                r.ResourceType = new AzureResourceType(ns, resourceType);
            }

            return r;
        }
    }
}
