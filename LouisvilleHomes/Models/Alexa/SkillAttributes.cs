using System.Collections.Generic;

namespace LouisvilleHomes.Models.Alexa
{
    public class SkillAttributes
    {
        public int SkillState { get; set; }

        public List<KeyValuePair<string, string>> KeyValuePairs { get; set; }
        
        public OutputSpeech OutputSpeech { get; set; }

        public SkillAttributes()
        {
            OutputSpeech = new OutputSpeech();
            KeyValuePairs = new List<KeyValuePair<string, string>>();
        }
    }
}