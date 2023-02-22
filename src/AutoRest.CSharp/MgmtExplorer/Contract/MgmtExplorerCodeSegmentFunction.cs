// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    public class MgmtExplorerCodeSegmentFunction
    {
        public const string FUNC_CODESEGMENT_CODE = "__FUNC_CODESEGMENT_CODE__";
        public string? Key { get; set; }
        public string? SuggestedName { get; set; }
        public MgmtExplorerCSharpType? Type { get; set; }
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string FunctionWrap { get; set; } = "";
        [YamlMember(ScalarStyle = ScalarStyle.Literal)]
        public string FunctionInvoke { get; set; } = "";

        public MgmtExplorerCodeSegmentFunction(string suggestedName, MgmtExplorerCSharpType? type)
        {
            Key = $"__FS__{suggestedName}__FE__";
            SuggestedName = suggestedName;
            Type = type;
        }

        public MgmtExplorerCodeSegmentFunction()
        {

        }
    }
}
