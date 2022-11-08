// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.CSharp.Common.Input;
using AutoRest.CSharp.Generation.Types;
using AutoRest.CSharp.Input;
using AutoRest.CSharp.Mgmt.AutoRest.PostProcess;
using AutoRest.CSharp.MgmtExplorer.Autorest;
using AutoRest.CSharp.MgmtExplorer.Contract;
using AutoRest.CSharp.Output.Models.Shared;

namespace AutoRest.CSharp.MgmtExplorer.Models
{
    /// <summary>
    /// The defined variable for parameter
    /// </summary>
    internal class MgmtExplorerParameter : MgmtExplorerPlaceHolder
    {
        public string CSharpName { get; init; }
        public string ModelName { get; init; }
        public string SerializerName { get; set; }
        public string Description { get; init; }
        public string RequestLocation { get; init; }
        public string? DefaultValue { get { return this.DefaultReplacement; } }

        public MgmtExplorerParameter(Parameter definition, RequestParameter requestParameter)
            : this(definition, requestParameter.Language.GetName(), requestParameter.Language.GetSerializerNameOrName())
        {
        }

        public MgmtExplorerParameter(Parameter definition, string modelName, string serializerName)
            : base($"__PS__{definition.Name}__PE__", definition.Type, GetDefaultValue(definition) ?? "__N/A__")
        {
            this.CSharpName = definition.Name;
            this.ModelName = modelName;
            this.SerializerName = serializerName;
            Description = definition.Description ?? string.Empty;
            this.RequestLocation = definition.RequestLocation.ToString();
        }



        public MgmtExplorerCodeSegmentParameter ToCodeSegmentParameter(bool includeSchema)
        {
            return new MgmtExplorerCodeSegmentParameter(
                this.Key,
                this.CSharpName,
                this.ModelName,
                this.SerializerName,
                new MgmtExplorerCSharpType(this.Type),
                this.Description,
                this.DefaultReplacement);
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
