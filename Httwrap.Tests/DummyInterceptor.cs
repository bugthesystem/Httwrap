using System.Net;
using System.Net.Http;
using Httwrap.Interception;
using Httwrap.Interface;

namespace Httwrap.Tests
{
    public class DummyInterceptor : IHttpInterceptor
    {
        private readonly IHttwrapClient _client;

        public void OnRequest(HttpRequestMessage request)
        {
            
        }

        public void OnResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            response.StatusCode = HttpStatusCode.Accepted;
        }
    }
}