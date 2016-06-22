using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Httwrap.Tests
{
    public class AuthenticationController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Token(TokenRequest request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                access_token = Guid.NewGuid().ToString()
            });
        }
    }
}