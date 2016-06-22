using System.Net.Http;

namespace Httwrap.Interception
{
    public interface IHttpInterceptor
    {
        void OnRequest(HttpRequestMessage request);

        void OnResponse(HttpRequestMessage request, HttpResponseMessage response);
    }
}