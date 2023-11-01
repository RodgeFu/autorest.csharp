// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.CSharp.Input;
using AutoRest.CSharp.MgmtExplorer.Autorest;
using AutoRest.CSharp.MgmtExplorer.Extensions;
using AutoRest.CSharp.Output.Models.Shared;
using AutoRest.SdkExplorer.Model.Code;

namespace AutoRest.CSharp.MgmtExplorer.Models
{
    /// <summary>
    /// The defined variable for parameter
    /// </summary>
    internal class MgmtExplorerParameter : MgmtExplorerPlaceHolder
    {
        public const string SOURCE_REQUEST_PATH = "RequestPath";

        public string CSharpName { get; init; }
        public string ModelName { get; init; }
        public string SerializerName { get; set; }
        public string Description { get; init; }
        public string RequestLocation { get; init; }
        public string? RequestPath { get; init; }
        public string? DefaultValue { get { return this.DefaultReplacement; } }
        public bool IsInPropertyBag { get; set; }
        /// <summary>
        /// Only support requestPath now, add more when needed
        /// </summary>
        public string? Source { get; set; }
        public string? SourceArg { get; set; }

        public MgmtExplorerParameter(Parameter definition, RequestParameter requestParameter, string requestPath)
            : this(definition, requestParameter.Language.GetName(), requestParameter.Language.GetSerializerNameOrName(), requestPath)
        {
        }

        public MgmtExplorerParameter(Parameter definition, string modelName, string serializerName, string requestPath)
            : base($"__PS__{definition.Name}__PE__", definition.Type, GetDefaultValue(definition) ?? "__N/A__")
        {
            this.CSharpName = definition.Name;
            this.ModelName = modelName;
            this.SerializerName = serializerName;
            Description = definition.Description?.ToString() ?? string.Empty;
            this.RequestLocation = definition.RequestLocation.ToString();
            this.RequestPath = requestPath;
            this.IsInPropertyBag = definition.IsPropertyBag;

            if (definition.RequestLocation == Common.Input.RequestLocation.Path)
            {
                string search = $"{{{this.CSharpName}}}";
                int index = this.RequestPath.IndexOf(search);
                if (index >= 0)
                {
                    this.Source = SOURCE_REQUEST_PATH;
                    this.SourceArg = requestPath.Substring(0, index + search.Length);
                }
                else
                {
                    throw new System.InvalidOperationException($"Can't find param in request Path. param: {this.CSharpName}, requestPath: {this.RequestPath}");
                }
            }
        }

        public ParameterDesc ToCodeSegmentParameter()
        {
            return new ParameterDesc()
            {
                Key = this.Key,
                SuggestedName = this.CSharpName,
                ModelName = this.ModelName,
                SerializerName = this.SerializerName,
                Type = this.Type.CreateSeTypeDesc(),
                Description = this.Description,
                DefaultValue = this.DefaultReplacement,
                RequestPath = this.RequestPath,
                Source = this.Source,
                SourceArg = this.SourceArg,
                IsInPropertyBag = this.IsInPropertyBag,
            };
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
                return "global::Azure.WaitUntil.Completed";
            }
            return value;
        }
    }
}
