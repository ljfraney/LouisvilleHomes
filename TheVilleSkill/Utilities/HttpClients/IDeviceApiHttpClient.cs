using TheVilleSkill.Models;
using TheVilleSkill.Models.Alexa;
using System.Threading.Tasks;

namespace TheVilleSkill.Utilities.HttpClients
{
    public interface IDeviceApiHttpClient
    {
        Task<ApiResponse<AddressApiResponse>> GetAddress(string requestUri, string accessToken);
    }
}