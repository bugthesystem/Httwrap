using Httwrap.Interface;
using Httwrap.Serialization;
using Newtonsoft.Json;

namespace Httwrap
{
    public static class JExtensions
    {
        static JExtensions()
        {
            Serializer = new JsonSerializerWrapper();
        }

        public static ISerializer Serializer { get; set; }

        public static string ToJson(this object @object)
        {
            return JsonConvert.SerializeObject(@object);
        }

        public static T ReadAs<T>(this IHttwrapResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Body);
        }
    }
}