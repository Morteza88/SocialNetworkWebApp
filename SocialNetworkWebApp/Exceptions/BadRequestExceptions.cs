using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Exceptions
{
    public class BadRequestExceptions : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";

        public BadRequestExceptions(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        public BadRequestExceptions(HttpStatusCode statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }

        public BadRequestExceptions(HttpStatusCode statusCode, Exception inner) : this(statusCode, inner.ToString()) { }

        public BadRequestExceptions(HttpStatusCode statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
        {
            this.ContentType = @"application/json";
        }

    }
}
