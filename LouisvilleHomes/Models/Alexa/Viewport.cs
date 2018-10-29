using Newtonsoft.Json;
using System.Collections.Generic;

namespace LouisvilleHomes.Models.Alexa
{
    public class Viewport
    {
        [JsonProperty("experiences")]
        public List<Experience> Experiences { get; set; }

        [JsonProperty("shape")]
        public string Shape { get; set; }

        [JsonProperty("pixelWidth")]
        public int PixelWidth { get; set; }

        [JsonProperty("pixelHeight")]
        public int PixelHeight { get; set; }

        [JsonProperty("dpi")]
        public int DPI { get; set; }

        [JsonProperty("currentPixelWidth")]
        public int CurrentPixelWidth { get; set; }

        [JsonProperty("currentPixelHeight")]
        public int CurrentPixelHeight { get; set; }

        [JsonProperty("touch")]
        public List<string> Touch { get; set; }

        [JsonProperty("keyboard")]
        public List<string> Keyboard { get; set; }
    }
}