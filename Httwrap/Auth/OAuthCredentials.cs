using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Httwrap.Auth
{
    public class OAuthCredentials : Credentials
    {
        private string _token;
        private readonly string _username;
        private readonly string _password;
        private readonly string _grantType;
        private readonly string _requestEndpoint;
        private readonly bool _isTls;

        public OAuthCredentials(string token, bool isTls = false)
        {
            Check.NotNullOrEmpty(token, "token");
            _token = token;
            _isTls = isTls;
        }

        public OAuthCredentials(string username, string password, string requestEndpoint)
        {
            Check.NotNullOrEmpty(username, "username");
            Check.NotNullOrEmpty(password, "password");
            Check.NotNullOrEmpty(requestEndpoint, "requestEndpoint");
            _username = username;
            _password = password;
            _requestEndpoint = requestEndpoint;
            _grantType = "password";

        }

        public override HttpClient BuildHttpClient(HttpMessageHandler httpHandler = null)
        {
            var httpClient = httpHandler != null ? new HttpClient(httpHandler) : new HttpClient();

            if (String.IsNullOrEmpty(_token))
            {
                FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"username", _username},
                    {"password", _password},
                    {"grant_type", _grantType}
                });

                HttpResponseMessage response = httpClient.PostAsync(_requestEndpoint, content).Result;
                Token token = new HttwrapResponse(response).ReadAs<Token>();
                _token = token.AccessToken;

            }

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            return httpClient;
        }

        public override bool IsTlsCredentials()
        {
            return _isTls;
        }
    }
}
