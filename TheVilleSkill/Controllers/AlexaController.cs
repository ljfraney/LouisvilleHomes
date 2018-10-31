using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TheVilleSkill.Models.Alexa;
using TheVilleSkill.Utilities;

namespace TheVilleSkill.Controllers
{
    [Route("alexa/v1")]
    [ApiController]
    public class AlexaController : Controller
    {
        private readonly AlexaSettings _alexaSettings;
        private readonly ISkillRequestHandler _skillRequestHandler;
        private readonly ILogger<AlexaController> _logger;

        public AlexaController(AlexaSettings alexaSettings, ISkillRequestHandler skillRequestHandler, ILogger<AlexaController> logger)
        {
            _alexaSettings = alexaSettings;
            _skillRequestHandler = skillRequestHandler;
            _logger = logger;
        }

        [HttpPost, Route("")]
        public async Task<AlexaResponse> Main([FromBody]AlexaRequest alexaRequest)
        {
            var alexaResponse = new AlexaResponse();

            try
            {
                var totalSeconds = (DateTime.UtcNow - alexaRequest.Request.Timestamp).TotalSeconds;
                if (totalSeconds >= _alexaSettings.TimeoutSeconds)
                {
                    alexaResponse.Response.ShouldEndSession = true;
                    alexaResponse.Response.OutputSpeech.Text = "It has been too long since your last response. Goodbye.";
                    return alexaResponse;
                }

                if (alexaRequest.Session.Application.ApplicationId != _alexaSettings.ApplicationId)
                {
                    alexaResponse.Response.ShouldEndSession = true;
                    alexaResponse.Response.OutputSpeech.Text = "This request appears to be intended for a different skill. Goodbye.";
                    return alexaResponse;
                }

                alexaResponse.SessionAttributes.SkillAttributes = alexaRequest.Session.Attributes.SkillAttributes;

                switch (alexaRequest.Request.Type)
                {
                    case "LaunchRequest":
                        alexaResponse = _skillRequestHandler.HandleLaunchRequest(alexaRequest, alexaResponse);
                        break;
                    case "IntentRequest":
                        alexaResponse = await _skillRequestHandler.HandleIntentRequest(alexaRequest, alexaResponse);
                        break;
                    case "SessionEndedRequest":
                        _skillRequestHandler.HandleSessionEndedRequest(alexaRequest);
                        break;
                    default:
                        alexaResponse.Response.ShouldEndSession = true;
                        alexaResponse.Response.OutputSpeech.Text = "I didn't understand. Goodbye.";
                        return alexaResponse;
                }

                //set value for repeat intent
                alexaResponse.SessionAttributes.SkillAttributes.OutputSpeech = alexaResponse.Response.OutputSpeech;
            }
            catch (Exception ex)
            {
                AlexaSafeExceptionHandler.HandleException(_logger, ex, alexaResponse);
            }

            return alexaResponse;
        }
    }
}