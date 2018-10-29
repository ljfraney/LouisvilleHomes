using Newtonsoft.Json;
using System;

namespace LouisvilleHomes.Models.Alexa
{
    public class Permissions
    {
        [Obsolete("Use Context.System.ApiAccessToken instead.")]
        [JsonProperty("consentToken")]
        public string ConsentToken { get; set; }
    }
}
