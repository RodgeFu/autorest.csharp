// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace azure_parameter_grouping.Models
{
    /// <summary> Parameter group. </summary>
    public partial class ParameterGroupingPostOptionalParameters
    {
        /// <summary> Initializes a new instance of <see cref="ParameterGroupingPostOptionalParameters"/>. </summary>
        public ParameterGroupingPostOptionalParameters()
        {
        }

        /// <summary> Initializes a new instance of <see cref="ParameterGroupingPostOptionalParameters"/>. </summary>
        /// <param name="customHeader"></param>
        /// <param name="query"> Query parameter with default. </param>
        internal ParameterGroupingPostOptionalParameters(string customHeader, int? query)
        {
            CustomHeader = customHeader;
            Query = query;
        }

        /// <summary> Gets or sets the custom header. </summary>
        public string CustomHeader { get; set; }
        /// <summary> Query parameter with default. </summary>
        public int? Query { get; set; }
    }
}
