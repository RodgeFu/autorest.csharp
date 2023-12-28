// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.SdkExplorer.Model.Code;

namespace AutoRest.SdkExplorer.Model.OpenAi
{
    public class AiChatResult
    {
        public ApiObjectPath? FunctionPath { get; set; }
        public AiChatMessage Message { get; set; }

        public AiChatResult(AiChatMessage msg, ApiObjectPath? functionpath = null)
        {
            this.Message = msg;
        }
    }
}
