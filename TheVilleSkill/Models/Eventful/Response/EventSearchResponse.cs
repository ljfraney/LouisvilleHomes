using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheVilleSkill.Utilities.JsonConverters;

namespace TheVilleSkill.Models.Eventful.Response
{
    public class EventSearchResponse
    {
        [JsonProperty("total_items")]
        public int TotalItems { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }

        [JsonProperty("page_count")]
        public int PageCount { get; set; }

        [JsonProperty("page_number")]
        public int PageNumber { get; set; }

        [JsonProperty("page_items")]
        public int? PageItems { get; set; }

        [JsonProperty("first_item")]
        public int? FirstItem { get; set; }

        [JsonProperty("last_item")]
        public int? LastItem { get; set; }
        
        [JsonProperty("search_time")]
        public double SearchTime { get; set; }

        [JsonConverter(typeof(SingleValueArrayConverter<Event>))]
        public List<Event> Events { get; set; }
    }
}
