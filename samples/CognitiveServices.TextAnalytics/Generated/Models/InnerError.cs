// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace CognitiveServices.TextAnalytics.Models
{
    /// <summary> The InnerError. </summary>
    public partial class InnerError
    {
        /// <summary> Initializes a new instance of InnerError. </summary>
        /// <param name="code"> Error code. </param>
        /// <param name="message"> Error message. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="message"/> is null. </exception>
        internal InnerError(InnerErrorCodeValue code, string message)
        {
            Argument.AssertNotNull(message, nameof(message));

            Code = code;
            Message = message;
            Details = new ChangeTrackingDictionary<string, string>();
        }

        /// <summary> Initializes a new instance of InnerError. </summary>
        /// <param name="code"> Error code. </param>
        /// <param name="message"> Error message. </param>
        /// <param name="details"> Error details. </param>
        /// <param name="target"> Error target. </param>
        /// <param name="innererror"> Inner error contains more specific information. </param>
        internal InnerError(InnerErrorCodeValue code, string message, IReadOnlyDictionary<string, string> details, string target, InnerError innererror)
        {
            Code = code;
            Message = message;
            Details = details;
            Target = target;
            Innererror = innererror;
        }

        /// <summary> Error code. </summary>
        public InnerErrorCodeValue Code { get; }
        /// <summary> Error message. </summary>
        public string Message { get; }
        /// <summary> Error details. </summary>
        public IReadOnlyDictionary<string, string> Details { get; }
        /// <summary> Error target. </summary>
        public string Target { get; }
        /// <summary> Inner error contains more specific information. </summary>
        public InnerError Innererror { get; }
    }
}
