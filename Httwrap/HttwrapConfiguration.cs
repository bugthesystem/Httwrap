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
            : this(basePath, httpHandler: null)
        {

        }

        public HttwrapConfiguration(string basePath, HttpMessageHandler httpHandler = null)
        {
            Check.NotNullOrEmpty(basePath, "The basePath may not be null or empty.");
            BasePath = basePath;
            _httpHandler = httpHandler;
            Credentials = new AnonymousCredentials();
        }

        public string BasePath { get; private set; }

        public ISerializer Serializer
        {
            get { return _serializer ?? (_serializer = new JsonSerializerWrapper()); }
            set { _serializer = value; }
        }

        public HttpClient GetHttpClient()
        {
            var client = Credentials.BuildHttpClient(_httpHandler);

            return client;
        }

        public Credentials Credentials { get; set; }
    }
}