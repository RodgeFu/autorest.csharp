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

        public AiChatSession()
        {
        }

        public AiChatSession(string sessionId)
        {
            this.SessionId = sessionId;
        }
    }
}
