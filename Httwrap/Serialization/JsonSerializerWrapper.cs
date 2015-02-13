using Httwrap.Interface;
using Newtonsoft.Json;

namespace Httwrap.Serialization
{
    internal class JsonSerializerWrapper : ISerializer
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}