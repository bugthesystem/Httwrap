using System.Net.Http;
using Httwrap.Auth;
using Httwrap.Interface;
using Httwrap.Serialization;

namespace Httwrap
{
    public class HttwrapConfiguration : IHttwrapConfiguration
    {
        private readonly HttpMessageHandler _httpHandler;
        private ISerializer _serializer;

        public HttwrapConfiguration(string basePath)
            : this(basePath, null)
        {
        }

        public HttwrapConfiguration(string basePath, HttpMessageHandler httpHandler = null)
        {
            Check.NotNullOrEmpty(basePath, "The basePath may not be null or empty.");
            BasePath = basePath;
            _httpHandler = httpHandler;
        }

        public string BasePath { get; protected set; }
        public Credentials Credentials { get; set; }

        public ISerializer Serializer
        {
            get { return _serializer ?? (_serializer = new NewtonsoftJsonSerializer()); }
            set { _serializer = value; }
        }

        public HttpClient GetHttpClient()
        {
            Credentials = Credentials ?? new AnonymousCredentials();
            var client = Credentials.BuildHttpClient(_httpHandler);

            return client;
        }
    }
}