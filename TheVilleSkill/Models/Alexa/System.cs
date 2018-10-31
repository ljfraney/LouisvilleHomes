using Newtonsoft.Json;

namespace TheVilleSkill.Models.Alexa
{
    public class System
    {
        [JsonProperty("application")]
        public Application Application { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("device")]
        public Device Device { get; set; }

        [JsonProperty("apiEndpoint")]
        public string ApiEndpoint { get; set; }

        [JsonProperty("apiAccessToken")]
        public string ApiAccessToken { get; set; }
    }
}
