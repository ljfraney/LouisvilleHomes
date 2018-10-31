using Newtonsoft.Json;

namespace TheVilleSkill.Models.Alexa
{
    public class User
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("permissions")]
        public Permissions Permissions { get; set; }
    }
}