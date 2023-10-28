// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace AutoRest.SdkExplorer.Model.OpenAi
{
    public class AiFunctionCall
    {
        public string Name { get; set; } = "";
        /// <summary>
        /// something like:
        /// "{\n  \"location\": \"San Diego\",\n  \"max_price\": 300,\n  \"features\": \"beachfront,free breakfast\"\n}"
        /// </summary>
        public string Arguments { get; set; } = "";
    }
}
