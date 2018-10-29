using System.Threading.Tasks;
using LouisvilleHomes.Models.Alexa;

namespace LouisvilleHomes.Utilities
{
    public interface ISkillRequestHandler
    {
        Task<AlexaResponse> HandleIntentRequest(AlexaRequest request, AlexaResponse response);

        Task<AlexaResponse> HandleLaunchRequest(AlexaRequest request, AlexaResponse response);

        Task<AlexaResponse> HandleSessionEndedRequest(AlexaRequest request, AlexaResponse response);
    }
}