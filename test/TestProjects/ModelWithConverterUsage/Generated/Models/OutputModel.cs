// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace ModelWithConverterUsage.Models
{
    /// <summary> The product documentation. </summary>
    public partial class OutputModel
    {
        /// <summary> Initializes a new instance of <see cref="OutputModel"/>. </summary>
        internal OutputModel()
        {
        }

        /// <summary> Initializes a new instance of <see cref="OutputModel"/>. </summary>
        /// <param name="outputModelProperty"> Constant string. </param>
        internal OutputModel(string outputModelProperty)
        {
            OutputModelProperty = outputModelProperty;
        }

        /// <summary> Constant string. </summary>
        public string OutputModelProperty { get; }
    }
}
