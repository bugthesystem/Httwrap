using Httwrap.Interface;
using Newtonsoft.Json;

namespace Httwrap
{
    public static class JExtensions
    {
        private static ISerializer serializer;

        public static ISerializer Serializer
        {
            get { return serializer ?? (serializer = new JsonSerializerWrapper()); }
            set { serializer = value; }
        }

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