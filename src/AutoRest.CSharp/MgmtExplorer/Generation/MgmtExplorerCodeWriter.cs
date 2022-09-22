// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoRest.CSharp.MgmtExplorer.Contract;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal class MgmtExplorerCodeWriter
    {
        public string Description { get; set; }
        public List<CodeSegment> CodeSegments { get; set; } = new List<CodeSegment>();

        public MgmtExplorerCodeWriter()
        {
        }

        public void AppendCodeSegment(CodeSegment seg)
        {
            // TODO: any check we want to do here?
            this.CodeSegments.Append(seg);
        }
    }
}
