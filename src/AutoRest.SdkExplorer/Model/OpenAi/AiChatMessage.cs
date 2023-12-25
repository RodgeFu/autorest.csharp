// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
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

        private static void validateRole(string role)
        {
            if (!new string[] { "system", "user", "function", "tool", "assistant" }.Contains(role))
            {
                throw new InvalidOperationException($"Invalid chat message role {role}, possible value: 'system', 'user', 'function', 'tool', 'assistant'");
            }
        }

        /// <summary>
        /// CreateMessageWithContent
        /// </summary>
        /// <param name="role">possible value: "system", "user", "function", "tool", "assistant"</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static AiChatMessage CreateMessageWithContent(string role, string content)
        {
            validateRole(role);
            return new AiChatMessage { Role = role, Content = content };
        }

        /// <summary>
        /// CreateMessageWithFuncCall
        /// </summary>
        /// <param name="role">possible value: "system", "user", "function", "tool", "assistant"</param>
        /// <param name="funcCall"></param>
        /// <returns></returns>
        public static AiChatMessage CreateMessageWithFuncCall(string role, AiFunctionCall funcCall)
        {
            validateRole(role);
            return new AiChatMessage
            {
                Role = role,
                Function_call = new AiFunctionCall()
                {
                    Name = funcCall.Name,
                    Arguments = funcCall.Arguments,
                }
            };
        }

        public static (AiChatMessage Ask, AiChatMessage Reply) CreateAskReplyWithContent(string ask, string reply)
        {
            var askMsg = CreateMessageWithContent("user", ask);
            var replyMsg = CreateMessageWithContent("assistant", reply);
            return (askMsg, replyMsg);
        }

        public static (AiChatMessage Ask, AiChatMessage Reply) CreateAskReplyWithFuncCall(string ask, AiFunctionCall func)
        {
            var askMsg = CreateMessageWithContent("user", ask);
            var replyMsg = CreateMessageWithFuncCall("assistant", func);
            return (askMsg, replyMsg);
        }
    }
}
