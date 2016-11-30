using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Httwrap.Interface;

namespace Httwrap
{
    internal class JsonRequestContent : IRequestContent
    {
        private const string JsonMimeType = "application/json";
        private readonly ISerializer _serializerWrapper;
        private readonly object _value;

        public JsonRequestContent(object value, ISerializer serializerWrapper)
        {
            if (EqualityComparer<object>.Default.Equals(value))
            {
                throw new ArgumentNullException("value");
            }

            if (serializerWrapper == null)
            {
                throw new ArgumentNullException("serializerWrapper");
            }

            _value = value;
            _serializerWrapper = serializerWrapper;
        }


        public HttpContent GetContent()
        {
            var serializedObject = _serializerWrapper.Serialize(_value);
            return new StringContent(serializedObject, Encoding.UTF8, JsonMimeType);
        }
    }
}