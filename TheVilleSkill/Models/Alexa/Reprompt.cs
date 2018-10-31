using Newtonsoft.Json;

namespace TheVilleSkill.Models.Alexa
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