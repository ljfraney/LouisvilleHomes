using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheVilleSkill.Models;
using TheVilleSkill.Models.Alexa;
using TheVilleSkill.Models.Alexa.CustomSlots;
using TheVilleSkill.Models.GoogleMaps;
using TheVilleSkill.Models.PVA;
using TheVilleSkill.Utilities.GoogleMaps;
using TheVilleSkill.Utilities.PVA;
using TheVilleSkill.Utilities.Repositories;

namespace TheVilleSkill.Utilities
{
    public class SkillRequestHandler : ISkillRequestHandler
    {
        private readonly ILogger<SkillRequestHandler> _logger;
        private readonly IPvaAddressLookup _pvaAddressLookup;
        private readonly IDeviceApiHttpClient _deviceApiHttpClient;
        private readonly IZipCodeRepository _zipCodeRepository;
        private readonly IMapImageUrlGenerator _mapImageUrlGenerator;
        private readonly IPhoneticAddressHelper _phoneticAddressHelper;

        public SkillRequestHandler(ILogger<SkillRequestHandler> logger,
            IPvaAddressLookup pvaAddressLookup, 
            IDeviceApiHttpClient deviceApiHttpClient, 
            IZipCodeRepository zipCodeRepository, 
            IMapImageUrlGenerator mapImageUrlGenerator,
            IPhoneticAddressHelper phoneticAddressHelper)
        {
            _logger = logger;
            _pvaAddressLookup = pvaAddressLookup;
            _deviceApiHttpClient = deviceApiHttpClient;
            _zipCodeRepository = zipCodeRepository;
            _mapImageUrlGenerator = mapImageUrlGenerator;
            _phoneticAddressHelper = phoneticAddressHelper;
        }

        public AlexaResponse HandleLaunchRequest(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                return HandleHomeRequest(request, response, true);
            }
            catch (Exception ex)
            {
                return AlexaSafeExceptionHandler.HandleException(_logger, ex, response);
            }
        }        

        public void HandleSessionEndedRequest(AlexaRequest request)
        {
            //Do any cleanup here. Per docs, "Your service cannot send back a response to a SessionEndedRequest"
        }

        public async Task<AlexaResponse> HandleIntentRequest(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                switch (request.Request.Intent.Name)
                {
                    case "AMAZON.NavigateHomeIntent":
                        response = HandleHomeRequest(request, response, false);
                        break;
                    case "WhoOwnsHouseIntent":
                        response = await HandleWhoOwnsHouseIntent(request, response, false);
                        break;
                    case "WhoOwnsMyHouseIntent":
                        response = await HandleWhoOwnsHouseIntent(request, response, true);
                        break;
                    case "WhoLivesAtHouseIntent":
                        response = await HandleWhoLivesAtHouseIntent(request, response, false);
                        break;
                    case "WhoLivesAtMyHouseIntent":
                        response = await HandleWhoLivesAtHouseIntent(request, response, true);
                        break;
                    case "PropertyValueIntent":
                        response = await HandlePropertyValueIntent(request, response, false);
                        break;
                    case "MyPropertyValueIntent":
                        response = await HandlePropertyValueIntent(request, response, true);
                        break;
                    case "AcreageIntent":
                        response = await HandleAcreageIntent(request, response, false);
                        break;
                    case "MyAcreageIntent":
                        response = await HandleAcreageIntent(request, response, true);
                        break;
                    case "AMAZON.FallbackIntent":
                        response = ProcessFallbackIntent(request, response);
                        break;
                    case "AMAZON.HelpIntent":
                        response = ProcessHelpIntent(request, response);
                        break;
                    case "AMAZON.CancelIntent":
                        response = ProcessStopIntent(request, response);
                        break;
                    case "AMAZON.StopIntent":
                        response = ProcessStopIntent(request, response);
                        break;                    
                }

                return response;
            }
            catch (Exception ex)
            {
                return AlexaSafeExceptionHandler.HandleException(_logger, ex, response);
            }
        }

        private AlexaResponse HandleHomeRequest(AlexaRequest request, AlexaResponse response, bool isLaunch)
        {
            response.Response.OutputSpeech.Text = "Welcome to the Ville Skill. ";
            response.Response.OutputSpeech.Text += "Ask me a question like, who owns 144 North Sixth Street?";
            response.Response.Reprompt.OutputSpeech.Text = "Go ahead and ask me a question. For help, just say help.";
            AddHelpCard(response);
            response.Response.ShouldEndSession = false;
            return response;
        }

        private async Task<AlexaResponse> HandleWhoOwnsHouseIntent(AlexaRequest request, AlexaResponse response, bool mine)
        {
            string address = null;

            if (mine)
            {
                var zipCodes = await _zipCodeRepository.GetZipCodes();
                var deviceAddress = await GetDeviceAddress(request);
                if (!MyAddress(response, deviceAddress, zipCodes, ref address))
                    return response;
            }
            else
            {
                var addressSlot = request.Request.Intent.Slots["address"].ToObject<AddressSlot>();
                address = addressSlot.Value;                    
            }

            var propertyInfo = await _pvaAddressLookup.GetPropertyInfo(address);

            if (!string.IsNullOrWhiteSpace(propertyInfo.Error))
            {
                response.Response.OutputSpeech.Text += $"Sorry, I wasn't able to find the owner of {address}.";
                response.Response.Reprompt.OutputSpeech.Text = "Ask me another question. For help, just say help.";
                AddUnknownAddressCard(response, address);
                response.Response.ShouldEndSession = false;
                return response;
            }

            var phoneticAddress = await _phoneticAddressHelper.GetPhoneticBaseAddress(propertyInfo.Address);
            response.Response.OutputSpeech.Text += $"The owner of {phoneticAddress} is {propertyInfo.Address.Owner}.";
            response.Response.Reprompt.OutputSpeech.Text = "Ask me another question. For help, just say help.";
            AddAddressCard(response, propertyInfo.Address.AddressDisplay, propertyInfo);
            response.Response.ShouldEndSession = false;

            return response;
        }

        private async Task<AlexaResponse> HandlePropertyValueIntent(AlexaRequest request, AlexaResponse response, bool mine)
        {
            string address = null;

            if (mine)
            {
                var zipCodes = await _zipCodeRepository.GetZipCodes();
                var deviceAddress = await GetDeviceAddress(request);
                if (!MyAddress(response, deviceAddress, zipCodes, ref address))
                    return response;
            }
            else
            {
                var addressSlot = request.Request.Intent.Slots["address"].ToObject<AddressSlot>();
                address = addressSlot.Value;
            }

            var propertyInfo = await _pvaAddressLookup.GetPropertyInfo(address);

            if (propertyInfo.Address.AssessedValue == null)
            {
                response.Response.OutputSpeech.Text = $"Sorry, I wasn't able to find the property valuation for {address}.";
                response.Response.Reprompt.OutputSpeech.Text = "Ask me another question. For help, just say help.";
                AddUnknownAddressCard(response, address);
                response.Response.ShouldEndSession = false;
                return response;
            }

            var phoneticAddress = await _phoneticAddressHelper.GetPhoneticBaseAddress(propertyInfo.Address);
            response.Response.OutputSpeech.Text = $"The assessed value of {phoneticAddress} is {propertyInfo.Address.AssessedValue} dollars.";
            response.Response.Reprompt.OutputSpeech.Text = "Ask me another question. For help, just say help.";
            AddAddressCard(response, propertyInfo.Address.AddressDisplay, propertyInfo);
            response.Response.ShouldEndSession = false;

            return response;
        }

        private async Task<AlexaResponse> HandleAcreageIntent(AlexaRequest request, AlexaResponse response, bool mine)
        {
            string address = null;

            if (mine)
            {
                var zipCodes = await _zipCodeRepository.GetZipCodes();
                var deviceAddress = await GetDeviceAddress(request);
                if (!MyAddress(response, deviceAddress, zipCodes, ref address))
                    return response;
            }
            else
            {
                var addressSlot = request.Request.Intent.Slots["address"].ToObject<AddressSlot>();
                address = addressSlot.Value;
            }

            var propertyInfo = await _pvaAddressLookup.GetPropertyInfo(address);

            if (propertyInfo.Address.Acreage == null)
            {
                response.Response.OutputSpeech.Text = $"Sorry, I wasn't able to find the size of {address}.";
                response.Response.Reprompt.OutputSpeech.Text = "Ask me another question. For help, just say help.";
                AddUnknownAddressCard(response, address);
                response.Response.ShouldEndSession = false;
                return response;
            }

            var phoneticAddress = await _phoneticAddressHelper.GetPhoneticBaseAddress(propertyInfo.Address);
            response.Response.OutputSpeech.Text = $"The property at {phoneticAddress} is {propertyInfo.Address.Acreage} acres.";
            response.Response.Reprompt.OutputSpeech.Text = "Ask me another question. For help, just say help.";
            AddAddressCard(response, propertyInfo.Address.AddressDisplay, propertyInfo);
            response.Response.ShouldEndSession = false;

            return response;
        }
        
        private async Task<AlexaResponse> HandleWhoLivesAtHouseIntent(AlexaRequest request, AlexaResponse response, bool mine)
        {
            response.Response.OutputSpeech.Text = "I can't tell you who lives at an address. I can only tell you who owns it. ";
            return await HandleWhoOwnsHouseIntent(request, response, mine);
        }

        private void AddAddressCard(AlexaResponse response, string address, PvaPropertyInfo propertyInfo)
        {
            response.Response.Card.Type = "Standard";
            response.Response.Card.Title = propertyInfo.Address.AddressDisplay;
            response.Response.Card.Text = $"Owner: {propertyInfo.Address.Owner}\r\nAssessed Value: ${string.Format("{0:n0}", propertyInfo.Address.AssessedValue)}\r\nAcres: {propertyInfo.Address.Acreage}";
            response.Response.Card.Image.SmallImageUrl = _mapImageUrlGenerator.GetMapUrl(address, MapSize.Small);
            response.Response.Card.Image.LargeImageUrl = _mapImageUrlGenerator.GetMapUrl(address, MapSize.Large);
        }

        private void AddUnknownAddressCard(AlexaResponse response, string address)
        {
            response.Response.Card.Type = "Simple";
            response.Response.Card.Title = "Couldn't Find Address";
            response.Response.Card.Content = $"Sorry, couldn't find\r\n{address}";
        }

        private void AddHelpCard(AlexaResponse response)
        {
            response.Response.Card.Type = "Simple";
            response.Response.Card.Title = "The Ville Skill";
            response.Response.Card.Content = "Who owns 411 S 4th St?\r\nWhat is my home worth?\r\nHow big is 301 River Rd?";
        }

        private AlexaResponse ProcessFallbackIntent(AlexaRequest request, AlexaResponse response)
        {
            response.Response.OutputSpeech.Text = "Sorry. I didn't understand the question. For help, just say help.";
            response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
            response.Response.ShouldEndSession = false;
            return response;
        }

        private AlexaResponse ProcessHelpIntent(AlexaRequest request, AlexaResponse response)
        {
            response.Response.OutputSpeech.Text = "Ask me a question like, who owns 411 South Fourth Street? What is my home worth? How big is 301 River Road?";
            response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
            AddHelpCard(response);
            response.Response.ShouldEndSession = false;
            return response;
        }

        private AlexaResponse ProcessStopIntent(AlexaRequest request, AlexaResponse response)
        {
            response.Response.OutputSpeech.Text = "Thanks for using The Ville Skill. Goodbye.";
            response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
            response.Response.ShouldEndSession = true;
            return response;
        }

        private async Task<ApiResponse<AddressApiResponse>> GetDeviceAddress(AlexaRequest request)
        {
            var apiAccessToken = request.Context.System.ApiAccessToken;
            var apiEndpoint = request.Context.System.ApiEndpoint;
            var deviceId = request.Context.System.Device.DeviceId;

            var addressApiUrl = $"{apiEndpoint}/v1/devices/{deviceId}/settings/address";

            return await _deviceApiHttpClient.GetAddress(addressApiUrl, apiAccessToken);
        }

        private bool MyAddress(AlexaResponse response, ApiResponse<AddressApiResponse> addressApiResponse, List<string> ZipCodes, ref string address)
        {
            if (addressApiResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                response.Response.OutputSpeech.Text = "In order to ask questions about your own address, you must grant permissions in your Alexa app.";
                response.Response.Reprompt.OutputSpeech.Text = "Ask me another question. For help, just say help.";
                response.Response.Card.Type = "AskForPermissionsConsent";
                response.Response.Card.Permissions.Add("read::alexa:device:all:address");
                response.Response.ShouldEndSession = false;
                return false;
            }

            var zipCode = addressApiResponse.Content.PostalCode?.Split("-".ToCharArray())[0];

            if (string.IsNullOrWhiteSpace(zipCode))
            {
                response.Response.OutputSpeech.Text = "Your address appears to be missing its zip code. This is needed to determine if your address is in Louisville Kentucky. You can update your address in the Alexa app.";
                response.Response.Reprompt.OutputSpeech.Text = "Ask me another question. For help, just say help.";
                response.Response.ShouldEndSession = false;
                return false;
            }

            if (!ZipCodes.Contains(zipCode))
            {
                response.Response.OutputSpeech.Type = "SSML";
                response.Response.OutputSpeech.Ssml = $"<speak>Your zip code, <say-as interpret-as=\"spell-out\">{zipCode}</say-as>, does not appear to be in Louisville Kentucky. Only addresses in Louisville Kentucky can be searched.</speak>";
                response.Response.Reprompt.OutputSpeech.Text = "Ask me another question. For help, just say help.";
                response.Response.ShouldEndSession = false;
                return false;
            }

            address = addressApiResponse.Content.AddressLine1;
            return true;
        }
    }
}