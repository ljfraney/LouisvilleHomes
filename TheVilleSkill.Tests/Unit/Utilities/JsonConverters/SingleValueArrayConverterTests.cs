using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TheVilleSkill.Models.Eventful.Response;
using Xunit;

namespace TheVilleSkill.Tests.Unit.Utilities.JsonConverters
{
    public class SingleValueArrayConverterTests
    {
        [Fact]
        [Trait("Category", "Unit")]
        public void WriteJson_DeserializesNullEvents()
        {
            const string eventSearchJson = "{\"total_items\":\"10297\",\"events\":null}";

            var eventSearchResponse = JsonConvert.DeserializeObject<EventSearchResponse>(eventSearchJson);

            Assert.Empty(eventSearchResponse.Events);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WriteJson_DeserializesNullEvent()
        {
            const string eventSearchJson = "{\"total_items\":\"10297\",\"events\":{\"event\":null}}";

            var eventSearchResponse = JsonConvert.DeserializeObject<EventSearchResponse>(eventSearchJson);

            Assert.Empty(eventSearchResponse.Events);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WriteJson_DeserializesSingleEvent()
        {
            const string eventSearchJson = "{\"total_items\":\"10297\",\"events\":{\"event\":{\"title\":\"Event #1\"}}}";

            var eventSearchResponse = JsonConvert.DeserializeObject<EventSearchResponse>(eventSearchJson);

            Assert.True(eventSearchResponse.Events.Count == 1);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WriteJson_DeserializesMultipleEvents()
        {
            const string eventSearchJson = "{\"total_items\":\"10297\",\"events\":{\"event\":[{\"title\":\"Event #1\"},{\"title\":\"Event #2\"}]}}";

            var eventSearchResponse = JsonConvert.DeserializeObject<EventSearchResponse>(eventSearchJson);

            Assert.True(eventSearchResponse.Events.Count == 2);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WriteJson_DeserializesNullPerformers()
        {
            const string eventSearchJson = "{\"total_items\":\"10297\",\"events\":{\"event\":{\"title\":\"Event #1\",\"performers\": null}}}";

            var eventSearchResponse = JsonConvert.DeserializeObject<EventSearchResponse>(eventSearchJson);

            Assert.Empty(eventSearchResponse.Events.First().Performers);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WriteJson_DeserializesNullPerformer()
        {
            const string eventSearchJson = "{\"total_items\":\"10297\",\"events\":{\"event\":{\"title\":\"Event #1\",\"performers\": {\"performer\":null}}}}";

            var eventSearchResponse = JsonConvert.DeserializeObject<EventSearchResponse>(eventSearchJson);

            Assert.Empty(eventSearchResponse.Events.First().Performers);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WriteJson_DeserializesSinglePerformer()
        {
            const string eventSearchJson = "{\"total_items\":\"10297\",\"events\":{\"event\":{\"title\":\"Event #1\",\"performers\": {\"performer\":{\"name\":\"Rick Starlight\"}}}}}";

            var eventSearchResponse = JsonConvert.DeserializeObject<EventSearchResponse>(eventSearchJson);

            Assert.True(eventSearchResponse.Events.First().Performers.Count == 1);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void WriteJson_DeserializesMultiplePerformers()
        {
            const string eventSearchJson = "{\"total_items\":\"10297\",\"events\":{\"event\":{\"title\":\"Event #1\",\"performers\": {\"performer\":[{\"name\":\"Rick Starlight\"},{\"name\":\"Garth Straight\"}]}}}}";

            var eventSearchResponse = JsonConvert.DeserializeObject<EventSearchResponse>(eventSearchJson);

            Assert.True(eventSearchResponse.Events.First().Performers.Count == 2);
        }
    }
}
