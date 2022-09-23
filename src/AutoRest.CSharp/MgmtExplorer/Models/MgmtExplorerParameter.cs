// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.Output.Models.Shared;

namespace AutoRest.CSharp.MgmtExplorer.Models
{
    /// <summary>
    /// The defined variable for parameter
    /// </summary>
    internal class MgmtExplorerParameter : MgmtExplorerPlaceHolder
    {
        public string ParameterName { get; init; }
        public string Description { get; init; }
        private MgmtExplorerCodeSegmentParameter _codeSegmentParameter;

        public MgmtExplorerParameter(Parameter definition)
            : this(definition.Name, definition.Type, GetDefaultValue(definition), definition.Description)
        {
        }

        public MgmtExplorerParameter(string parameterName, CSharpType type, string? defaultValue, string? description)
            : base($"__PS__{parameterName}__PE__", type, defaultValue ?? "__N/A__")
        {
            ParameterName = parameterName;
            Description = description ?? string.Empty;
            this._codeSegmentParameter = new MgmtExplorerCodeSegmentParameter(
                this.Key,
                this.ParameterName,
                this.Type.ToCodeSegmentCSharpType(),
                this.Description,
                this.DefaultReplacement);
        }

        public MgmtExplorerCodeSegmentParameter AsCodeSegmentParameter()
        {
            return this._codeSegmentParameter;
        }

        private static string? GetDefaultValue(Parameter param)
        {
            string? value = null;
            if (param.DefaultValue != null)
            {
                if (param.DefaultValue.Value.Value == null || param.DefaultValue.Value.IsNewInstanceSentinel)
                {
                    return "default";
                }
                else
                {
                    return param.DefaultValue.Value.Value.ToString()!;
                }
            }
            else if (param.Type.Equals(typeof(Azure.WaitUntil)))
            {
                return "Azure.WaitUntil.Completed";
            }
            return value;
        }
    }
}
