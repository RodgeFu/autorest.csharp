// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AutoRest.SdkExplorer.Model.Code
{
    public class FunctionDesc
    {
        public const string FUNC_CODESEGMENT_CODE = "__FUNC_CODESEGMENT_CODE__";

        public string? Key { get; set; }
        public string? SuggestedName { get; set; }
        public TypeDesc? Type { get; set; }
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string? FunctionWrap { get; set; }
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string? FunctionInvoke { get; set; }

        public FunctionDesc()
        {

        }

        public FunctionDesc(string suggestedName, TypeDesc type)
        {
            Key = $"__FS__{suggestedName}__FE__";
            SuggestedName = suggestedName;
            Type = type;
        }
    }
}
