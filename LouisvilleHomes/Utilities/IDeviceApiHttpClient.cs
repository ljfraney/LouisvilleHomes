using LouisvilleHomes.Models;
using LouisvilleHomes.Models.Alexa;
using System.Threading.Tasks;

namespace LouisvilleHomes.Utilities
{
    public interface IDeviceApiHttpClient
    {
        Task<ApiResponse<AddressApiResponse>> GetAddress(string requestUri, string accessToken);
    }
}