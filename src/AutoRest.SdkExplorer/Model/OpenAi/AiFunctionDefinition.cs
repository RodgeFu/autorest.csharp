// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace AutoRest.SdkExplorer.Model.OpenAi
{
    public class AiFunctionDefinition
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        /// <summary>
        /// something like:
        /// {\"name\":\"search_hotels\",\"description\":\"Retrieves hotels from the search index based on the parameters provided\",\"parameters\":{\"type\":\"object\",\"properties\":{\"location\":{\"type\":\"string\",\"description\":\"The location of the hotel (i.e. Seattle, WA)\"},\"max_price\":{\"type\":\"number\",\"description\":\"The maximum price for the hotel\"},\"features\":{\"type\":\"string\",\"description\":\"A comma separated list of features (i.e. beachfront, free wifi, etc.)\"}},\"required\":[\"location\"]}}
        /// </summary>
        public string ParametersAsString { get; set; } = "";
    }
}
