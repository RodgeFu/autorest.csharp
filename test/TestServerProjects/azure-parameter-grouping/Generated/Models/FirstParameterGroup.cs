// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace azure_parameter_grouping.Models
{
    /// <summary> Parameter group. </summary>
    public partial class FirstParameterGroup
    {
        /// <summary> Initializes a new instance of <see cref="FirstParameterGroup"/>. </summary>
        public FirstParameterGroup()
        {
        }

        /// <summary> Initializes a new instance of <see cref="FirstParameterGroup"/>. </summary>
        /// <param name="headerOne"></param>
        /// <param name="queryOne"> Query parameter with default. </param>
        internal FirstParameterGroup(string headerOne, int? queryOne)
        {
            HeaderOne = headerOne;
            QueryOne = queryOne;
        }

        /// <summary> Gets or sets the header one. </summary>
        public string HeaderOne { get; set; }
        /// <summary> Query parameter with default. </summary>
        public int? QueryOne { get; set; }
    }
}
