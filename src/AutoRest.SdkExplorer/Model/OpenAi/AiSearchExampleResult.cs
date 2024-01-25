// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
using System.Collections.Generic;
using AutoRest.SdkExplorer.Model.Code;

namespace AutoRest.SdkExplorer.Model.OpenAi
{
    public class AiSearchExampleResult
    {
        private static bool _useCamelInSearchFieldName = true;
        private static string GetSearchFieldName(string fieldName)
        {
            if (_useCamelInSearchFieldName)
            {
                if (string.IsNullOrEmpty(fieldName))
                    return fieldName;
                return char.ToLower(fieldName[0]) + fieldName.Substring(1);
            }
            else
                return fieldName;
        }

        public static readonly string[] SEARCH_FIELDS = new string[]
        {
            GetSearchFieldName(nameof(AiSearchExampleResult.ServiceName)),
            GetSearchFieldName(nameof(AiSearchExampleResult.ResourceName)),
            GetSearchFieldName(nameof(AiSearchExampleResult.OperationName)),
            GetSearchFieldName(nameof(AiSearchExampleResult.ExampleName)),
            GetSearchFieldName(nameof(AiSearchExampleResult.SdkPackageName)),
            GetSearchFieldName(nameof(AiSearchExampleResult.SdkPackageVersion)),
            GetSearchFieldName(nameof(AiSearchExampleResult.SdkFullUniqueName)),
            GetSearchFieldName(nameof(AiSearchExampleResult.OriginalFileNameWithoutExtension)),
            GetSearchFieldName(nameof(AiSearchExampleResult.SampleCode)),
        };

        public string ServiceName { get; private set; }
        public string ResourceName { get; private set; }
        public string OperationName { get; private set; }
        public string ExampleName { get; private set; }
        public string SdkPackageName { get; private set; }
        public string SdkPackageVersion { get; private set; }
        public string SdkFullUniqueName { get; private set; }
        public string OriginalFileNameWithoutExtension { get; private set; }
        public string SampleCode { get; private set; }
        public double Score { get; private set; }

        public AiSearchExampleResult(ApiDesc desc, string exampleName, string originalFileNameWithoutExtension, double score)
        {
            this.ServiceName = desc.ServiceName;
            this.ResourceName = desc.ResourceName;
            this.OperationName = desc.OperationName;
            this.SdkPackageName = desc.SdkPackageName;
            this.SdkPackageVersion = desc.SdkPackageVersion;
            this.ExampleName = exampleName;
            this.SdkFullUniqueName = desc.FullUniqueName;
            this.OriginalFileNameWithoutExtension = originalFileNameWithoutExtension;
            this.SampleCode = "No Sample Code available";
            this.Score = score;
        }

        /// <summary>
        /// The caller needs to make sure proper fields are available in the searchDocument
        /// </summary>
        /// <param name="searchDocument"></param>
        /// <param name="score"></param>
        public AiSearchExampleResult(IDictionary<string, object> searchDocument, double score)
        {
            ServiceName = searchDocument[GetSearchFieldName(nameof(AiSearchExampleResult.ServiceName))].ToString() ?? "";
            ResourceName = searchDocument[GetSearchFieldName(nameof(AiSearchExampleResult.ResourceName))].ToString() ?? "";
            OperationName = searchDocument[GetSearchFieldName(nameof(AiSearchExampleResult.OperationName))].ToString() ?? "";
            ExampleName = searchDocument[GetSearchFieldName(nameof(AiSearchExampleResult.ExampleName))].ToString() ?? "";
            SdkPackageName = searchDocument[GetSearchFieldName(nameof(AiSearchExampleResult.SdkPackageName))].ToString() ?? "";
            SdkPackageVersion = searchDocument[GetSearchFieldName(nameof(AiSearchExampleResult.SdkPackageVersion))].ToString() ?? "";
            SdkFullUniqueName = searchDocument[GetSearchFieldName(nameof(AiSearchExampleResult.SdkFullUniqueName))].ToString() ?? "";
            OriginalFileNameWithoutExtension = searchDocument[GetSearchFieldName(nameof(AiSearchExampleResult.OriginalFileNameWithoutExtension))].ToString() ?? "";
            SampleCode = searchDocument[GetSearchFieldName(nameof(AiSearchExampleResult.SampleCode))].ToString() ?? "";
            Score = score;
        }
    }
}
