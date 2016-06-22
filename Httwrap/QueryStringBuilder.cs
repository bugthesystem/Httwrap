using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Httwrap
{
    internal class QueryStringBuilder : IQueryStringBuilder
    {
        public string BuildFrom<T>(T payload, string separator = ",")
        {
            if (payload == null)
                throw new ArgumentNullException("payload");

            // Get all properties on the object
            var properties = payload.GetType().GetProperties()
                .Where(info => info.CanRead)
                .Where(info => !HasIgnoreDataMemberAttribute(info))
                .Where(info => info.GetValue(payload, null) != null)
                .ToDictionary(info => GetName(info), x => x.GetValue(payload, null));

            // Get names for all IEnumerable properties (excl. string)
            var propertyNames = properties
                .Where(pair => !(pair.Value is string) && pair.Value is IEnumerable)
                .Select(pair => pair.Key)
                .ToList();

            // Concat all IEnumerable properties into a comma separated string
            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                    ? valueType.GetGenericArguments()[0]
                    : valueType.GetElementType();
                if (valueElemType.IsPrimitive || valueElemType == typeof(string))
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

        private bool HasIgnoreDataMemberAttribute(PropertyInfo info)
        {
            var attributes =
                info.GetCustomAttributes(typeof(IgnoreDataMemberAttribute), true)
                    .Cast<IgnoreDataMemberAttribute>()
                    .ToList();

            if (attributes.Any())
            {
                var attribute = attributes.FirstOrDefault();

                if (attribute != null)
                {
                    return true;
                }
            }

            return false;
        }

        private string GetName(PropertyInfo info)
        {
            IEnumerable<DataMemberAttribute> attributes =
                info.GetCustomAttributes(typeof(DataMemberAttribute), true).Cast<DataMemberAttribute>().ToList();

            if (attributes.Any())
            {
                var attribute = attributes.FirstOrDefault();

                if (attribute != null)
                {
                    return attribute.Name;
                }
            }

            return info.Name;
        }
    }
}