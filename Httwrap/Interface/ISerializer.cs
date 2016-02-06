using Newtonsoft.Json;

namespace Httwrap.Interface
{
    public interface ISerializer
    {
        T Deserialize<T>(string json, JsonSerializerSettings settings = null);
        string Serialize<T>(T value, JsonSerializerSettings settings = null);
    }
}