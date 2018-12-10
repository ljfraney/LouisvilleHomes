using Newtonsoft.Json;
using System.Collections.Generic;

namespace TheVilleSkill.Models.Eventful.Response
{
    public class EventsList
    {
        [JsonProperty("event")]
        public List<Event> Events { get; set; }
    }
}
