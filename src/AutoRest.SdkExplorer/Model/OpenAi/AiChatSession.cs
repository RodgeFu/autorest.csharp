// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace AutoRest.SdkExplorer.Model.OpenAi
{
    public class AiChatSession
    {
        public string SessionId { get; set; } = string.Empty;
        public List<AiChatMessage> ChatMessages { get; set; } = new List<AiChatMessage>();
        public List<AiFunctionDefinition> Functions { get; set; } = new List<AiFunctionDefinition>();

        public float? NucleusSamplingFactor { get; set; }
        public float? PresencePenalty { get; set; }
        public float? FrequencyPenalty { get; set; }
        public float? Temperature { get; set; }

        public AiChatSession()
        {
        }

        public AiChatSession(string sessionId)
        {
            this.SessionId = sessionId;
        }
    }
}
