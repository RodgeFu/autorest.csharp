// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using AutoRest.SdkExplorer.Model.Code;

namespace AutoRest.SdkExplorer.Model.OpenAi
{
    public class AiChatContext
    {
        public List<AiChatMessage> ChatMessages { get; set; } = new List<AiChatMessage>();
        public List<ApiObjectPath> FunctionPaths { get; set; } = new List<ApiObjectPath>();

        public int? Seed { get; set; }
        public float? NucleusSamplingFactor { get; set; }
        public float? PresencePenalty { get; set; }
        public float? FrequencyPenalty { get; set; }
        public float? Temperature { get; set; }

        public AiChatContext()
        {
        }
    }
}
