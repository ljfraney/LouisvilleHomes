using Newtonsoft.Json;
using System.Collections.Generic;

namespace LouisvilleHomes.Models.Alexa
{
    [JsonObject("card")]
    public class Card
    {
        public Card()
        {
            Type = "Simple";
            Image = new Image();
            Permissions = new List<string>();
        }

        /// <summary>
        /// Options include Simple, Standard, and LinkAccount.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Text should be used for Standard cards.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Content should be used for Simple cards.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Image should only be used with Standard cards.
        /// </summary>
        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("permissions")]
        public List<string> Permissions { get; set; }
    }
}