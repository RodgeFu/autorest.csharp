// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text.Json.Serialization;

namespace AutoRest.SdkExplorer.Model.OpenAi
{
    public class AiChatMessage
    {
        /// <summary>
        /// possible value: "system", "user", "function", "tool", "assistant"
        /// </summary>
        public string Role { get; set; } = "system";
        public string? Content { get; set; }
        [JsonPropertyName("function_call")]
        public AiFunctionCall? Function_call { get; set; }
    }
}
