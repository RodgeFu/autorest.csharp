// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    internal class MgmtExplorerCode
    {
        public string Description { get; set; } = string.Empty;
        private List<MgmtExplorerCodeSegment> CodeSegments { get; } = new List<MgmtExplorerCodeSegment>();

        public MgmtExplorerCode()
        {
        }

        public void AddCodeSegment(MgmtExplorerCodeSegment newSegment)
        {
            this.CodeSegments.Add(newSegment);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.CodeSegments.Select(s => s.Code.ToString()));
        }
    }
}
