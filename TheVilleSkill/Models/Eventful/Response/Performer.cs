using Newtonsoft.Json;

namespace TheVilleSkill.Models.Eventful.Response
{
    public class Performer
    {
        public string Creator { get; set; }

        public string Linker { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Id { get; set; }

        [JsonProperty("short_bio")]
        public string ShortBio { get; set; }
    }
}
