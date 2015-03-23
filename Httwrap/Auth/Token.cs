using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Httwrap.Auth
{
    class Token
    {
        public Token()
        {
        }

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
    }
}
