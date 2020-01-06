// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace xml_service.Models.V100
{
    /// <summary> CORS is an HTTP feature that enables a web application running under one domain to access resources in another domain. Web browsers implement a security restriction known as same-origin policy that prevents a web page from calling APIs in a different domain; CORS provides a secure way to allow one domain (the origin domain) to call APIs in another domain. </summary>
    public partial class CorsRule
    {
        /// <summary> The origin domains that are permitted to make a request against the storage service via CORS. The origin domain is the domain from which the request originates. Note that the origin must be an exact case-sensitive match with the origin that the user age sends to the service. You can also use the wildcard character &apos;*&apos; to allow all origin domains to make requests via CORS. </summary>
        public string AllowedOrigins { get; set; }
        /// <summary> The methods (HTTP request verbs) that the origin domain may use for a CORS request. (comma separated). </summary>
        public string AllowedMethods { get; set; }
        /// <summary> the request headers that the origin domain may specify on the CORS request. </summary>
        public string AllowedHeaders { get; set; }
        /// <summary> The response headers that may be sent in the response to the CORS request and exposed by the browser to the request issuer. </summary>
        public string ExposedHeaders { get; set; }
        /// <summary> The maximum amount time that a browser should cache the preflight OPTIONS request. </summary>
        public int MaxAgeInSeconds { get; set; }
    }
}
