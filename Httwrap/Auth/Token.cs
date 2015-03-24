using Newtonsoft.Json;

namespace Httwrap.Auth
{
    internal class Token
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
    }
}
