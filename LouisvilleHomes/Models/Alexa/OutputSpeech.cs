using Newtonsoft.Json;

namespace LouisvilleHomes.Models.Alexa
{
    [JsonObject("outputSpeech")]
    public class OutputSpeech
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("ssml")]
        public string Ssml { get; set; }

        public OutputSpeech()
        {
            Type = "PlainText";
        }
    }
}