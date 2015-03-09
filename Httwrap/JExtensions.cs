using Httwrap.Interface;
using Httwrap.Serialization;

namespace Httwrap
{
    public static class JExtensions
    {
        static JExtensions()
        {
            Serializer = new NewtonsoftJsonSerializer();
        }

        public static ISerializer Serializer { get; set; }

        public static string ToJson(this object @object)
        {
            return Serializer.Serialize(@object);
        }

        public static T ReadAs<T>(this IHttwrapResponse response)
        {
            return Serializer.Deserialize<T>(response.Body);
        }
    }
}