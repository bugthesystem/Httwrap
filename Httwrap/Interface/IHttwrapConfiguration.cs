using System.Net.Http;
using Httwrap.Auth;

namespace Httwrap.Interface
{
    public interface IHttwrapConfiguration
    {
        string BasePath { get; }
        ISerializer Serializer { get; }
        Credentials Credentials { get; set; }
        HttpClient GetHttpClient();
    }
}