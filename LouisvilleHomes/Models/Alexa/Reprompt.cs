using Newtonsoft.Json;

namespace LouisvilleHomes.Models.Alexa
{
    [JsonObject("reprompt")]
    public class Reprompt
    {
        [JsonProperty("outputSpeech")]
        public OutputSpeech OutputSpeech { get; set; }

        public Reprompt()
        {
            OutputSpeech = new OutputSpeech();
        }
    }
}