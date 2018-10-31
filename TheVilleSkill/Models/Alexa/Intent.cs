using Newtonsoft.Json.Linq;

namespace TheVilleSkill.Models.Alexa
{
    public class Intent
    {
        public string Name { get; set; }

        public JObject Slots { get; set; }
    }
}