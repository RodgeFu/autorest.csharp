// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using AutoRest.CSharp.Generation.Writers;
using AutoRest.CSharp.Output.Models.Shared;

namespace AutoRest.CSharp.MgmtExplorer.Models
{
    /// <summary>
    /// The defined variable for parameter
    /// </summary>
    internal class MgmtExplorerParameterVariable
    {
        public CodeWriterDeclaration Variable { get; set; }
        public Parameter ParameterDefinition { get; set; }

        public string Value_PlaceHolder { get { return $"{{{ParameterDefinition.Name}_ParamValue}}"; } }
        public string Value_Default
        {
            get
            {
                string value;
                if (ParameterDefinition.DefaultValue != null)
                {
                    if (ParameterDefinition.DefaultValue.Value.Value == null || ParameterDefinition.DefaultValue.Value.IsNewInstanceSentinel)
                    {
                        return "default";
                    }
                    else
                    {
                        return ParameterDefinition.DefaultValue.Value.Value.ToString()!;
                    }
                }
                else if (ParameterDefinition.Type.Equals(typeof(Azure.WaitUntil)))
                {
                    return "Azure.WaitUntil.Completed";
                }
                else
                {
                    value = Value_PlaceHolder;
                }
                return value;
                //return ParameterDefinition.DefaultValue?.Value?.ToString() ?? Value_PlaceHolder;
            }
        }
        public bool HasDefaultValue { get { return ParameterDefinition.DefaultValue != null; } }

        public MgmtExplorerParameterVariable(Parameter definition)
            : this($"{definition.Name}Var", definition)
        {
        }

        public MgmtExplorerParameterVariable(string variableName, Parameter definition)
            : this(new CodeWriterDeclaration(variableName), definition)
        {
        }

        public MgmtExplorerParameterVariable(CodeWriterDeclaration variable, Parameter definition)
        {
            Variable = variable;
            ParameterDefinition = definition;
        }

        public string NameInMethodInvoke(bool useNamedParameterIfOptional = true)
        {
            if (useNamedParameterIfOptional && this.HasDefaultValue)
                return $"{this.ParameterDefinition.Name}: {this.Variable.ActualName}";
            else
                return this.Variable.ActualName;
        }
    }
}
