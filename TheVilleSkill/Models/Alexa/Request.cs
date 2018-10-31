using System;

namespace TheVilleSkill.Models.Alexa
{
    public class Request
    {
        public string Type { get; set; }

        public string RequestId { get; set; }

        public DateTime Timestamp { get; set; }

        public Intent Intent { get; set; }

        public string Locale { get; set; }

        public Request()
        {
            Intent = new Intent();
        }
    }
}