using Newtonsoft.Json;

namespace LouisvilleHomes.Models.Alexa
{
    public class AddressApiResponse
    {
        [JsonProperty("stateOrRegion")]
        public string StateOrRegion { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("addressLine2")]
        public string AddressLine2 { get; set; }

        [JsonProperty("addressLine3")]
        public string AddressLine3 { get; set; }

        [JsonProperty("districtOrCounty")]
        public string DistrictOrCounty { get; set; }
    }
}
