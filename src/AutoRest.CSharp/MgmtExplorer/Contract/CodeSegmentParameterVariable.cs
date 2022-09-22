// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using AutoRest.CSharp.MgmtExplorer.Models;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    internal class CodeSegmentParameterVariable : CodeSegmentVariable
    {
        public object Schema { get; set; }

        public CodeSegmentParameterVariable(CodeSegmentVariableKey key, MgmtExplorerVariable var, object schema)
            :base(key, var)
        {
            this.Schema = schema;
        }
    }
}
