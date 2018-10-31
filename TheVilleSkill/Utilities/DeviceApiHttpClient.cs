using TheVilleSkill.Models;
using TheVilleSkill.Models.Alexa;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace TheVilleSkill.Utilities
{
    public class DeviceApiHttpClient : IDeviceApiHttpClient
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILogger<DeviceApiHttpClient> _logger;

        public DeviceApiHttpClient(HttpClient httpClient, ILogger<DeviceApiHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ApiResponse<AddressApiResponse>> GetAddress(string requestUri, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync(requestUri);

            var addressResponse = new ApiResponse<AddressApiResponse>
            {
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                StatusCode = response.StatusCode
            };

            if (!addressResponse.IsSuccessStatusCode)
                return addressResponse;

            addressResponse.Content = JsonConvert.DeserializeObject<AddressApiResponse>(await response.Content.ReadAsStringAsync());
            
            return addressResponse;
        }
    }
}
