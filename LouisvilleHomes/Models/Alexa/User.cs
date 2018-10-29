using Newtonsoft.Json;

namespace LouisvilleHomes.Models.Alexa
{
    public class User
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("permissions")]
        public Permissions Permissions { get; set; }
    }
}