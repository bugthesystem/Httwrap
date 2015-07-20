using Httwrap.Interface;
using Newtonsoft.Json;

namespace Httwrap.Serialization
{
    public class NewtonsoftJsonSerializer : ISerializer
    {
        public T Deserialize<T>(string json, JsonSerializerSettings settings = null)
        {
            if (settings != null)
            {
                return JsonConvert.DeserializeObject<T>(json, settings);
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public string Serialize<T>(T value, JsonSerializerSettings settings = null)
        {
            if (settings != null)
            {
                return JsonConvert.SerializeObject(value, settings);
            }

            return JsonConvert.SerializeObject(value);
        }
    }
}