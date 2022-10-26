// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using Azure.Core;

namespace CognitiveServices.TextAnalytics.Models
{
    /// <summary> Contains an input document to be analyzed by the service. </summary>
    public partial class MultiLanguageInput
    {
        /// <summary> Initializes a new instance of MultiLanguageInput. </summary>
        /// <param name="id"> A unique, non-empty document identifier. </param>
        /// <param name="text"> The input text to process. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> or <paramref name="text"/> is null. </exception>
        public MultiLanguageInput(string id, string text)
        {
            Argument.AssertNotNull(id, nameof(id));
            Argument.AssertNotNull(text, nameof(text));

            Id = id;
            Text = text;
        }

        /// <summary> A unique, non-empty document identifier. </summary>
        public string Id { get; }
        /// <summary> The input text to process. </summary>
        public string Text { get; }
        /// <summary> (Optional) This is the 2 letter ISO 639-1 representation of a language. For example, use &quot;en&quot; for English; &quot;es&quot; for Spanish etc. If not set, use &quot;en&quot; for English as default. </summary>
        public string Language { get; set; }
    }
}
