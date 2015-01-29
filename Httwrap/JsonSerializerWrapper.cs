using Httwrap.Interface;
using Newtonsoft.Json;

namespace Httwrap
{
    internal class JsonSerializerWrapper : ISerializer
    {
        public T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string SerializeObject<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}