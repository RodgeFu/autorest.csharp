// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoRest.CSharp.Generation.Types;

namespace AutoRest.CSharp.MgmtExplorer.Contract
{
    internal static class MgmtExplorerCodeSegmentUtility
    {
        public static MgmtExplorerCodeSegmentCSharpType ToCodeSegmentCSharpType(this CSharpType st)
        {
            if (st.IsGenericType == false && st.Arguments?.Length == 0)
                return new MgmtExplorerCodeSegmentCSharpType(st.Name, st.Namespace, false, Array.Empty<MgmtExplorerCodeSegmentCSharpType>());
            else if (st.IsGenericType == true && st.Arguments?.Length > 0)
            {
                var arguments = st.Arguments.Select(t => ToCodeSegmentCSharpType(t)).ToArray();
                return new MgmtExplorerCodeSegmentCSharpType(st.Name, st.Namespace, true, arguments);
            }
            else
            {
                throw new InvalidOperationException("IsGenericeType is true while argument is null or zero for: " + st.Name);
            }
        }
    }
}
