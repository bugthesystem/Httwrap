using System.Net.Http;
using Httwrap.Interface;
using Httwrap.Serialization;

namespace Httwrap
{
    public class HttwrapConfiguration : IHttwrapConfiguration
    {
        private readonly HttpMessageHandler _httpHandler;
        private ISerializer _serializer;

        public HttwrapConfiguration(string basePath)
        {
            Check.NotNullOrEmpty(basePath, "The basePath may not be null or empty.");
            BasePath = basePath;
        }

        public HttwrapConfiguration(string basePath, HttpMessageHandler httpHandler = null)
            : this(basePath)
        {
            _httpHandler = httpHandler;
        }

        public string BasePath { get; private set; }

        public ISerializer Serializer
        {
            get { return _serializer ?? (_serializer = new JsonSerializerWrapper()); }
            set { _serializer = value; }
        }

        public HttpClient GetHttpClient()
        {
            var client = _httpHandler != null ? new HttpClient(_httpHandler) : new HttpClient();

            return client;
        }
    }
}