using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Httwrap.Auth
{
    public class OAuthCredentials : Credentials
    {
        private readonly string _token;
        private readonly bool _isTls;

        public OAuthCredentials(string token, bool isTls = false)
        {
            Check.NotNullOrEmpty(token, "token");
            _token = token;
            _isTls = isTls;
        }

        public override HttpClient BuildHttpClient(HttpMessageHandler httpHandler = null)
        {
            HttpClient httpClient = httpHandler != null ? new HttpClient(httpHandler) : new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", _token));

            return httpClient;
        }

        public override bool IsTlsCredentials()
        {
            return _isTls;
        }
    }
}
