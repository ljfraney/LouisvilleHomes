using LouisvilleHomes.Models.Alexa;
using LouisvilleHomes.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace LouisvilleHomes.Controllers
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

        [HttpGet, Route("test")]
        public async Task Test()
        {
            System.Diagnostics.Debugger.Break();
        }

        [HttpPost, Route("")]
        public async Task<AlexaResponse> Main([FromBody]AlexaRequest alexaRequest)
        {
            var response = new AlexaResponse();

            try
            {
                var totalSeconds = (DateTime.UtcNow - alexaRequest.Request.Timestamp).TotalSeconds;
                if (totalSeconds >= _alexaSettings.TimeoutSeconds)
                {
                    response.Response.ShouldEndSession = true;
                    response.Response.OutputSpeech.Text = "It has been too long since your last response. Goodbye.";
                    return response;
                }

                if (alexaRequest.Session.Application.ApplicationId != _alexaSettings.ApplicationId)
                {
                    response.Response.ShouldEndSession = true;
                    response.Response.OutputSpeech.Text = "This request appears to be intended for a different skill. Goodbye.";
                    return response;
                }

                response.SessionAttributes.SkillAttributes = alexaRequest.Session.Attributes.SkillAttributes;

                switch (alexaRequest.Request.Type)
                {
                    case "LaunchRequest":
                        response = await _skillRequestHandler.HandleLaunchRequest(alexaRequest, response);
                        break;
                    case "IntentRequest":
                        response = await _skillRequestHandler.HandleIntentRequest(alexaRequest, response);
                        break;
                    case "SessionEndedRequest":
                        response = await _skillRequestHandler.HandleSessionEndedRequest(alexaRequest, response);
                        break;
                    default:
                        response.Response.ShouldEndSession = true;
                        response.Response.OutputSpeech.Text = "I didn't understand. Goodbye.";
                        return response;
                }

                //set value for repeat intent
                response.SessionAttributes.SkillAttributes.OutputSpeech = response.Response.OutputSpeech;
            }
            catch (Exception ex)
            {
                AlexaSafeExceptionHandler.HandleException(_logger, ex, response);
            }

            return response;
        }
    }
}