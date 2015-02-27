using System.Net.Http;

namespace Httwrap.Auth
{
    public abstract class Credentials
    {
        public abstract HttpClient BuildHttpClient(HttpMessageHandler httpHandler);

        public abstract bool IsTlsCredentials();
    }
}