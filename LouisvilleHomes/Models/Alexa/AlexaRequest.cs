using Newtonsoft.Json;

namespace LouisvilleHomes.Models.Alexa
{
    public class AlexaRequest
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("session")]
        public Session Session { get; set; }

        [JsonProperty("context")]
        public Context Context { get; set; }

        [JsonProperty("request")]
        public Request Request { get; set; }        

        public AlexaRequest()
        {
            Version = "1.0";
            Session = new Session();
            Request = new Request();
        }
    }
}