using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TheVilleSkill.Models.Eventful;
using TheVilleSkill.Models.Eventful.Response;

namespace TheVilleSkill.Utilities.HttpClients
{
    public class EventfulHttpClient : IEventfulHttpClient
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILogger<EventfulHttpClient> _logger;
        protected readonly EventfulSettings _settings;
        protected readonly ITimeService _timeService;

        public EventfulHttpClient(HttpClient httpClient, ILogger<EventfulHttpClient> logger, EventfulSettings settings, ITimeService timeService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.BaseUrl);
            _logger = logger;
            _settings = settings;
            _timeService = timeService;
        }

        public async Task<EventSearchResponse> GetEvents() => await GetEvents(TimePeriod.Future);

        public async Task<EventSearchResponse> GetEventsToday() => await GetEvents(TimePeriod.Today);

        public async Task<EventSearchResponse> GetEventsOnDate(DateTime date) => await GetEvents(TimePeriod.OnDate(date));

        public async Task<EventSearchResponse> GetEventsInTheNextXDays(int days) => await GetEvents(TimePeriod.NextXDays(days, _timeService.Today));

        public async Task<EventSearchResponse> GetEventsForDateRange(DateTime fromDate, DateTime toDate) => await GetEvents(TimePeriod.DateRange(fromDate, toDate));

        private async Task<EventSearchResponse> GetEvents(string timePeriod)
        {
            var url = $"events/search?l=louisville&page_size=50&date={timePeriod}&sort_order=popularity&app_key={_settings.ApiKey}";
            var strResponse = await _httpClient.GetStringAsync(url);
            var response = JsonConvert.DeserializeObject<EventSearchResponse>(strResponse);
            return response;
        }
    }
}
