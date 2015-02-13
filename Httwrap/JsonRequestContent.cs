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

        public JsonRequestContent(object val, ISerializer serializerWrapper)
        {
            if (EqualityComparer<object>.Default.Equals(val))
            {
                throw new ArgumentNullException("val");
            }

            if (serializerWrapper == null)
            {
                throw new ArgumentNullException("serializerWrapper");
            }

            Value = val;
            SerializerWrapper = serializerWrapper;
        }

        private object Value { get; set; }
        private ISerializer SerializerWrapper { get; set; }

        public HttpContent GetContent()
        {
            var serializedObject = SerializerWrapper.Serialize(Value);
            return new StringContent(serializedObject, Encoding.UTF8, JsonMimeType);
        }
    }
}