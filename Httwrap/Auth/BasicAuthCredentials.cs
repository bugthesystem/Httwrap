using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Httwrap.Auth
{
    public class BasicAuthCredentials : Credentials
    {
        private readonly bool _isTls;
        private readonly string _password;
        private readonly string _username;

        public BasicAuthCredentials(string username, string password, bool isTls = false)
        {
            Check.NotNullOrEmpty(username, "username");
            Check.NotNullOrEmpty(password, "password");

            _username = username;
            _password = password;
            _isTls = isTls;
        }

        public override HttpClient BuildHttpClient(HttpMessageHandler httpHandler = null)
        {
            var httpClient = httpHandler != null ? new HttpClient(httpHandler) : new HttpClient();

            var authString =
                Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _username, _password)));

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);

            return httpClient;
        }

        public override bool IsTlsCredentials()
        {
            return _isTls;
        }
    }
}