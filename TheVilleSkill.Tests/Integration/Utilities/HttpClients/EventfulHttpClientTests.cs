using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TheVilleSkill.Models.Eventful;
using TheVilleSkill.Utilities;
using TheVilleSkill.Utilities.HttpClients;
using Xunit;

namespace TheVilleSkill.Tests.Integration.Utilities.HttpClients
{
    public class EventfulHttpClientTests
    {
        private static HttpClient _httpClient;
        private readonly EventfulSettings _settings;

        public EventfulHttpClientTests()
        {
            _httpClient = new HttpClient();

            var configuration = ConfigurationInitializer.InitConfiguration();
            _settings = new EventfulSettings
            {
                BaseUrl = configuration["EventfulSettings:BaseUrl"],
                ApiKey = configuration["EventfulSettings:ApiKey"]
            };
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetEvents_ReturnsData()
        {
            var client = new EventfulHttpClient(_httpClient, It.IsAny<ILogger<EventfulHttpClient>>(), _settings, null);
            var result = await client.GetEvents();
            Assert.NotNull(result);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetEventsToday_ReturnsData()
        {
            var client = new EventfulHttpClient(_httpClient, It.IsAny<ILogger<EventfulHttpClient>>(), _settings, null);
            var result = await client.GetEventsToday();
            Assert.NotNull(result);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetEventsOnDate_ReturnsData()
        {
            var client = new EventfulHttpClient(_httpClient, It.IsAny<ILogger<EventfulHttpClient>>(), _settings, null);
            var onDate = DateTime.Today.AddDays(5);
            var result = await client.GetEventsOnDate(onDate);
            Assert.NotNull(result);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetEventsInTheNextXDays_ReturnsData()
        {
            const int numberOfDays = 5;
            var timeServiceMock = new Mock<ITimeService>();
            timeServiceMock.SetupGet(ts => ts.Today).Returns(DateTime.Today);
            var client = new EventfulHttpClient(_httpClient, It.IsAny<ILogger<EventfulHttpClient>>(), _settings, timeServiceMock.Object);
            var result = await client.GetEventsInTheNextXDays(numberOfDays);
            Assert.NotNull(result);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task GetEventsForDateRange_ReturnsData()
        {
            var fromDate = DateTime.Today.AddDays(5);
            var toDate = DateTime.Today.AddDays(8);
            var client = new EventfulHttpClient(_httpClient, It.IsAny<ILogger<EventfulHttpClient>>(), _settings, null);
            var result = await client.GetEventsForDateRange(fromDate, toDate);
            Assert.NotNull(result);

        }
    }
}
