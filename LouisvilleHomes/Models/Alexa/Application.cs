using Newtonsoft.Json;

namespace LouisvilleHomes.Models.Alexa
{
    public class Application
    {
        [JsonProperty("applicationId")]
        public string ApplicationId { get; set; }
    }
}