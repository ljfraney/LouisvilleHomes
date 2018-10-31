using Newtonsoft.Json;

namespace TheVilleSkill.Models.Alexa
{
    public class Application
    {
        [JsonProperty("applicationId")]
        public string ApplicationId { get; set; }
    }
}