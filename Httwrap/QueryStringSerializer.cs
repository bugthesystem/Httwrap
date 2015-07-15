using System;
using System.Collections;
using System.Linq;

namespace Httwrap
{
    internal class QueryStringSerializer : IQueryStringSerializer
    {
        public string Serialize<T>(T payload, string separator = ",")
        {
            if (payload == null)
                throw new ArgumentNullException("payload");

            // Get all properties on the object
            var properties = payload.GetType().GetProperties()
                .Where(x => x.CanRead)
                .Where(x => x.GetValue(payload, null) != null)
                .ToDictionary(x => x.Name, x => x.GetValue(payload, null));

            // Get names for all IEnumerable properties (excl. string)
            var propertyNames = properties
                .Where(x => !(x.Value is string) && x.Value is IEnumerable)
                .Select(x => x.Key)
                .ToList();

            // Concat all IEnumerable properties into a comma separated string
            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                    ? valueType.GetGenericArguments()[0]
                    : valueType.GetElementType();
                if (valueElemType.IsPrimitive || valueElemType == typeof (string))
                {
                    var enumerable = properties[key] as IEnumerable;
                    if (enumerable != null) properties[key] = string.Join(separator, enumerable.Cast<object>());
                }
            }

            // Concat all key/value pairs into a string separated by ampersand
            return string.Join("&",
                properties.Select(
                    x => string.Concat(Uri.EscapeDataString(x.Key), "=", Uri.EscapeDataString(x.Value.ToString()))));
        }
    }
}