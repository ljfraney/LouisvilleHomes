using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TheVilleSkill.Utilities.JsonConverters;

namespace TheVilleSkill.Models.Eventful.Response
{
    public class Event
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("stop_time")]
        public DateTime? StopTime { get; set; }

        [JsonProperty("olson_path")]
        public string OlsonPath { get; set; }

        [JsonProperty("venue_id")]
        public string VenueId { get; set; }

        [JsonProperty("venue_url")]
        public string VenueUrl { get; set; }

        [JsonProperty("venue_name")]
        public string VenueName { get; set; }

        [JsonProperty("venue_display")]
        public int VenueDisplay { get; set; }

        [JsonProperty("venue_address")]
        public string VenueAddress { get; set; }

        [JsonProperty("city_name")]
        public string CityName { get; set; }

        [JsonProperty("region_name")]
        public string RegionName { get; set; }

        [JsonProperty("region_abbr")]
        public string RegionAbbr { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("country_abbr")]
        public string ContryAbbr { get; set; }

        [JsonProperty("country_abbr2")]
        public string CountryAbbr2 { get; set; }

        [JsonProperty("all_day")]
        public int AllDay { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [JsonProperty("geocode_type")]
        public string GeocodeType { get; set; }

        [JsonProperty("trackback_count")]
        public int? TrackbackCount { get; set; }

        [JsonProperty("calendar_count")]
        public int? CalendarCount { get; set; }

        [JsonProperty("comment_count")]
        public int? CommentCount { get; set; }

        [JsonProperty("link_count")]
        public int? LinkCount { get; set; }

        [JsonProperty("watching_count")]
        public int? WatchingCount { get; set; }

        [JsonProperty("going_count")]
        public int? GoingCount { get; set; }

        public DateTime Created { get; set; }

        public string Owner { get; set; }

        public DateTime Modified { get; set; }

        public dynamic Groups { get; set; }

        public int Privacy { get; set; }

        [JsonConverter(typeof(SingleValueArrayConverter<Performer>))]
        public List<Performer> Performers { get; set; }

        [JsonProperty("recur_string")]
        public string RecurString { get; set; }

        public dynamic Calendars { get; set; }

        public Going Going { get; set; }

        public Image Image { get; set; }

        [JsonProperty("tz_id")]
        public string TzId { get; set; }

        [JsonProperty("tz_country")]
        public string TzCountry { get; set; }

        [JsonProperty("tz_olson_path")]
        public string TzOlsonPath { get; set; }

        [JsonProperty("tz_city")]
        public string TzCity { get; set; }
    }
}
