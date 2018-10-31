using Newtonsoft.Json;

namespace TheVilleSkill.Models.Alexa
{
    public class Device
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("supportedInterfaces")]
        public dynamic SupportedInterfaces { get; set; }
    }
}