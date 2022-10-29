﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.MgmtExplorer.Models;

namespace AutoRest.CSharp.MgmtExplorer.Generation
{
    internal class MgmtExplorerCodeGenContext
    {
        public MgmtExplorerCodeDesc ExplorerCode { get; init; }
        public MgmtExplorerCodeSegmentWriter CodeSegmentWriter { get; set; }
        private MgmtExplorerCodeSegment CurCodeSegment { get; set; }
        public MgmtExplorerApiDesc ApiDesc { get; init; }

        public MgmtExplorerVariable? ArmClientVar
        {
            get { return GetVariable(nameof(ArmClientVar)); }
            set { SetVariable(nameof(ArmClientVar), value); }
        }
        public MgmtExplorerVariable? ProviderHostVar
        {
            get { return GetVariable(nameof(ProviderHostVar)); }
            set { SetVariable(nameof(ProviderHostVar), value); }
        }
        public MgmtExplorerVariable? ProviderVar
        {
            get { return GetVariable(nameof(ProviderVar)); }
            set { SetVariable(nameof(ProviderVar), value); }
        }
        public MgmtExplorerVariable? ResultVar
        {
            get { return GetVariable(nameof(ResultVar)); }
            set { SetVariable(nameof(ResultVar), value); }
        }

        private Dictionary<string, MgmtExplorerVariable> Variables { get; set; } = new Dictionary<string, MgmtExplorerVariable>();

        public MgmtExplorerCodeGenContext(MgmtExplorerApiDesc apiDesc, string firstCodeSegmentKey, string firstCodeSegmentSuggstedName)
        {
            this.ApiDesc = apiDesc;
            this.CodeSegmentWriter = new MgmtExplorerCodeSegmentWriter();
            this.CurCodeSegment = new MgmtExplorerCodeSegment(firstCodeSegmentKey, firstCodeSegmentSuggstedName);
            this.ExplorerCode = new MgmtExplorerCodeDesc(apiDesc);
        }

        public MgmtExplorerVariable? GetVariable(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return this.Variables.TryGetValue(key, out MgmtExplorerVariable? r) ? r : null;
        }

        public void SetVariable(string key, MgmtExplorerVariable? declaration)
        {
            if (declaration == null)
            {
                this.Variables.Remove(key);
            }
            else
            {
                this.Variables.Add(key, declaration);
            }
        }

        public void PushCodeSegment(Action<MgmtExplorerCodeSegment> processOldSegment, string newSegmentKey = "", string newSuggestedName = "")
        {
            this.CodeSegmentWriter.WriteToCodeSegment(this.CurCodeSegment);
            processOldSegment(this.CurCodeSegment);
            this.ExplorerCode.AddCodeSegment(this.CurCodeSegment);

            this.CurCodeSegment = new MgmtExplorerCodeSegment(newSegmentKey, newSuggestedName);
            this.CodeSegmentWriter = new MgmtExplorerCodeSegmentWriter();
        }

        //public List<MgmtExplorerCodeSegmentParameter> GetAllCodeSegementParameters()
        //{
        //    return this.ExplorerCode.CodeSegments.SelectMany(c => c.Parameters).ToList();
        //}
    }
}
