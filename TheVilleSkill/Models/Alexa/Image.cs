using Newtonsoft.Json;

namespace TheVilleSkill.Models.Alexa
{
    [JsonObject("image")]
    public class Image
    {
        [JsonProperty("smallImageUrl")]
        public string SmallImageUrl { get; set; }

        [JsonProperty("largeImageUrl")]
        public string LargeImageUrl { get; set; }
    }
}