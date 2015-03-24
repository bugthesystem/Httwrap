using Newtonsoft.Json;

namespace Httwrap.Tests
{
    // Test class
    public class TokenRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
    }
}