using Newtonsoft.Json;

namespace Httwrap
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object @object)
        {
            return JsonConvert.SerializeObject(@object);
        }

        public static T ReadAs<T>(this HttwrapResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Body);
        }
    }
}