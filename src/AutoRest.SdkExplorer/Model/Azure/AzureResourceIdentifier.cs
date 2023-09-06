// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoRest.SdkExplorer.Model.Azure
{
    public class AzureResourceIdentifier
    {
        // save the raw id only for readability and troubleshooting purpose
        public string RawId { get; set; }
        // segments and action are source of truth
        public List<AzureResourceIdentifierSegment> ResourceSegments { get; set; }
        public string Action { get; set; }

        public string GetId()
        {
            string resourcePart = string.Join("/", this.ResourceSegments.Select(s => s.ToString()));
            string actionPart = this.Action == null ? "" : this.Action;

            if (string.IsNullOrEmpty(resourcePart))
                return string.Empty;
            if (string.IsNullOrEmpty(actionPart))
                return $"/{resourcePart}";
            else
                return $"/{resourcePart}/{actionPart}";
        }
        public string? GetSubscriptionId() { return this.GetResourceTypeValue("subscriptions"); }
        public string? GetResourceGroupName() { return this.GetResourceTypeValue("resourcegroups"); }
        public AzureResourceType? GetResourceType() {
            int providerIndex = GetResourceTypeIndex("providers");
            if (providerIndex >= 0)
            {
                string ns = this.ResourceSegments[providerIndex].Value;
                string type = string.Join("/", this.ResourceSegments.Take(new Range(providerIndex + 1, this.ResourceSegments.Count)).Select(s => s.Type));
                return new AzureResourceType(ns, type);
            }
            return null;
        }
        public string? GetLastResourceName() { return this.ResourceSegments.Count > 0? this.ResourceSegments[^1].Value : null; }

        /// <summary>
        /// just for json/yaml ser/des
        /// </summary>
        private AzureResourceIdentifier()
        {
            this.RawId = string.Empty;
            this.ResourceSegments = new List<AzureResourceIdentifierSegment>();
            this.Action = string.Empty;
        }

        public AzureResourceIdentifier(string id)
        {
            this.RawId = id;
            this.ResourceSegments = new List<AzureResourceIdentifierSegment>();
            this.Action = string.Empty;
            this.Parse(id);
        }

        public string? GetResourceTypeValue(string resourceTypeName)
        {
            return this.ResourceSegments.FirstOrDefault(s => s.Type == resourceTypeName)?.Value;
        }

        public int GetResourceTypeIndex(string resourceTypeName)
        {
            int i = 0;
            foreach (var seg in this.ResourceSegments)
            {
                if (seg.Type == resourceTypeName)
                    return i;
                i++;
            }
            return -1;
        }

        private List<AzureResourceIdentifierSegment> CreateSegmentsList(string[] arr, out string action)
        {
            int segLen = arr.Length / 2;
            List<AzureResourceIdentifierSegment> segs = new List<AzureResourceIdentifierSegment>();
            for (int i = 0; i < segLen; i++)
            {
                segs.Add(new AzureResourceIdentifierSegment(arr[2 * i], arr[2 * i + 1]));
            }
            action = (arr.Length % 2 != 0) ? arr[^1] : string.Empty;
            return segs;
        }

        private void Parse(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            string[] arr = id.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (arr.Length == 0)
                throw new ArgumentException("At least one segment is expected in id. Id=" + id);

            var segList = CreateSegmentsList(arr, out string action);
            this.ResourceSegments = segList;
            this.Action = action;
        }
    }
}
