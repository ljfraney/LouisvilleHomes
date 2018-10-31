using Newtonsoft.Json;

namespace TheVilleSkill.Models.Alexa
{
    [JsonObject("response")]
    public class Response
    {
        [JsonProperty("outputSpeech")]
        public OutputSpeech OutputSpeech { get; set; }

        [JsonProperty("card")]
        public Card Card { get; set; }

        [JsonProperty("reprompt")]
        public Reprompt Reprompt { get; set; }

        [JsonProperty("shouldEndSession")]
        public bool ShouldEndSession { get; set; }

        public Response()
        {
            OutputSpeech = new OutputSpeech();
            Card = new Card();
            Reprompt = new Reprompt();
            ShouldEndSession = false;
        }
    }
}