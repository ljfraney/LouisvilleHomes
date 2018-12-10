using Newtonsoft.Json;
using System.Collections.Generic;

namespace TheVilleSkill.Models.Eventful.Response
{
    public class PerformerList
    {
        [JsonProperty("performer")]
        public List<Performer> Performers { get; set; }
    }
}
