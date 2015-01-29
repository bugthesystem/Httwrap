using Httwrap.Interface;

namespace Httwrap.Tests
{
    public class TestConfiguration : IHttwrapConfiguration
    {
        public TestConfiguration(string basePath, ISerializer serializer = null)
        {
            Check.NotNullOrEmpty(basePath, "The basePath may not be null or empty.");
            BasePath = basePath;
            Serializer = serializer;
        }

        public string BasePath { get; private set; }
        public ISerializer Serializer { get; private set; }
    }
}