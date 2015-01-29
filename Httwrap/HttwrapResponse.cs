using System.Net;

namespace Httwrap
{
    public class HttwrapResponse
    {
        public HttwrapResponse(HttpStatusCode statusCode, string body)
        {
            StatusCode = statusCode;
            Body = body;
        }

        public HttpStatusCode StatusCode { get; private set; }
        public string Body { get; private set; }

        public virtual bool Success
        {
            get { return StatusCode == HttpStatusCode.OK; }
        }
    }
}