using System.Threading.Tasks;
using TheVilleSkill.Models.Alexa;

namespace TheVilleSkill.Utilities
{
    public interface ISkillRequestHandler
    {
        AlexaResponse HandleLaunchRequest(AlexaRequest request, AlexaResponse response);

        Task<AlexaResponse> HandleIntentRequest(AlexaRequest request, AlexaResponse response);

        void HandleSessionEndedRequest(AlexaRequest request);
    }
}