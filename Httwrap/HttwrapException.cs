using System;
using System.Net;

namespace Httwrap
{
    public class HttwrapException : Exception
    {
        public HttwrapException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public HttwrapException(string message)
            : base(message)
        {
        }
    }

    public class HttwrapHttpException : HttwrapException
    {
        public HttwrapHttpException(HttpStatusCode statusCode, string responseBody)
            : base(string.Format("Request responded with status code={0}, response={1}", statusCode, responseBody))
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
        }

        public HttpStatusCode StatusCode { get; private set; }
        public string ResponseBody { get; private set; }
    }
}