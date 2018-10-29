using LouisvilleHomes.Models.Alexa;
using LouisvilleHomes.Models.Alexa.CustomSlots;
using LouisvilleHomes.Models.GoogleMaps;
using LouisvilleHomes.Models.PVA;
using LouisvilleHomes.Utilities.GoogleMaps;
using LouisvilleHomes.Utilities.PVA;
using LouisvilleHomes.Utilities.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LouisvilleHomes.Utilities
{
    public class SkillRequestHandler : ISkillRequestHandler
    {
        private readonly ILogger<SkillRequestHandler> _logger;
        private readonly IPvaAddressLookup _pvaAddressLookup;
        private readonly IDeviceApiHttpClient _deviceApiHttpClient;
        private readonly IZipCodeRepository _zipCodeRepository;
        private readonly IMapImageUrlGenerator _mapImageUrlGenerator;

        public SkillRequestHandler(ILogger<SkillRequestHandler> logger,
            IPvaAddressLookup pvaAddressLookup, 
            IDeviceApiHttpClient deviceApiHttpClient, 
            IZipCodeRepository zipCodeRepository, 
            IMapImageUrlGenerator mapImageUrlGenerator)
        {
            _logger = logger;
            _pvaAddressLookup = pvaAddressLookup;
            _deviceApiHttpClient = deviceApiHttpClient;
            _zipCodeRepository = zipCodeRepository;
            _mapImageUrlGenerator = mapImageUrlGenerator;
        }

        public async Task<AlexaResponse> HandleLaunchRequest(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                response.Response.OutputSpeech.Text = "Welcome to Louisville Homes. Ask me a question like, who owns 144 North Sixth Street?";
                response.Response.Reprompt.OutputSpeech.Text = "Go ahead and ask me a question.";
                response.Response.Card.Type = "Simple";
                response.Response.Card.Title = "Louisville Homes";
                response.Response.Card.Content = "Who owns 144 North 6th Street?\r\nWho owns my house?\r\nWhat's 1234 Main Street worth?";
                response.Response.ShouldEndSession = false;
                return response;
            }
            catch (Exception ex)
            {
                return AlexaSafeExceptionHandler.HandleException(_logger, ex, response);
            }
        }

        public async Task<AlexaResponse> HandleIntentRequest(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                switch (request.Request.Intent.Name)
                {
                    case "WhoOwnsIntent":
                        response = await HandleWhoOwnsIntent(request, response);
                        break;
                    case "WhoOwnsMyIntent":
                        response = await HandleWhoOwnsMyIntent(request, response);
                        break;
                    case "WhoLivesIntent":
                        response = await HandleWhoLivesIntent(request, response);
                        break;
                    case "PropertyValueIntent":
                        response = await HandlePropertyValueIntent(request, response);
                        break;
                    case "AcreageIntent":
                        response = await HandleAcreageIntent(request, response);
                        break;
                        //case "AMAZON.RepeatIntent":
                        //    response = await ProcessRepeatIntent(request, response);
                        //    break;
                        //case "YearIntent":
                        //    response = await ProcessYearIntent(request, response);
                        //    break;
                        //case "ColorIntent":
                        //    response = await ProcessColorIntent(request, response);
                        //    break;
                        //case "OrdinalIntent":
                        //    response = await ProcessOrdinalIntent(request, response);
                        //    break;
                        //case "RepeatWiresIntent":
                        //    response = await ProcessRepeatWiresIntent(request, response);
                        //    break;
                        //case "AMAZON.NoIntent":
                        //    response = await ProcessNoIntent(request, response);
                        //    break;
                        //case "AMAZON.YesIntent":
                        //    response = await ProcessYesIntent(request, response);
                        //    break;
                        //case "AMAZON.Date":
                        //    response = await ProcessDateIntent(request, response);
                        //    break;
                        //case "AMAZON.Time":
                        //    response = await ProcessTimeIntent(request, response);
                        //    break;
                        //case "AMAZON.CancelIntent":
                        //    response = await ProcessCancelIntent(request, response);
                        //    break;
                        //case "AMAZON.StopIntent":
                        //    response = await ProcessCancelIntent(request, response);
                        //    break;
                        //case "AMAZON.HelpIntent":
                        //    response = await ProcessHelpIntent(request, response);
                        //    break;
                }

                return response;
            }
            catch (Exception ex)
            {
                return AlexaSafeExceptionHandler.HandleException(_logger, ex, response);
            }
        }

        private async Task<AlexaResponse> HandleWhoOwnsIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                var addressSlot = request.Request.Intent.Slots["address"].ToObject<AddressSlot>();
                var address = addressSlot.Value;
                var propertyInfo = await _pvaAddressLookup.GetPropertyInfo(address);

                if (!string.IsNullOrWhiteSpace(propertyInfo.Error))
                {
                    response.Response.OutputSpeech.Text = $"Sorry, I wasn't able to find the owner of {address}.";
                    response.Response.Reprompt.OutputSpeech.Text = $"Sorry, I wasn't able to find the owner of {address}.";
                    response.Response.ShouldEndSession = false;
                    return response;
                }

                response.Response.OutputSpeech.Text = $"The owner of {address} is {propertyInfo.Owners}.";
                response.Response.Reprompt.OutputSpeech.Text = $"The owner of {address} is {propertyInfo.Owners}.";
                AddAddressCard(response, address, propertyInfo);
                response.Response.ShouldEndSession = false;

                return response;
            }
            catch (Exception ex)
            {
                return AlexaSafeExceptionHandler.HandleException(_logger, ex, response);
            }
        }

        private async Task<AlexaResponse> HandleWhoOwnsMyIntent(AlexaRequest request, AlexaResponse response)
        {
            var apiAccessToken = request.Context.System.ApiAccessToken;
            var apiEndpoint = request.Context.System.ApiEndpoint;
            var deviceId = request.Context.System.Device.DeviceId;

            var addressApiUrl = $"{apiEndpoint}/v1/devices/{deviceId}/settings/address";

            var addressApiResponse = await _deviceApiHttpClient.GetAddress(addressApiUrl, apiAccessToken);

            if (addressApiResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                response.Response.OutputSpeech.Text = "In order to ask questions about your own address, you must grant permissions in your Alexa app.";
                response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
                response.Response.Card.Type = "AskForPermissionsConsent";
                response.Response.Card.Permissions.Add("read::alexa:device:all:address");
                response.Response.ShouldEndSession = false;
                return response;
            }

            var zipCode = addressApiResponse.Content.PostalCode?.Split("-".ToCharArray())[0];

            if (string.IsNullOrWhiteSpace(zipCode))
            {
                response.Response.OutputSpeech.Text = "Your address appears to be missing its zip code. This is needed to determine if your address is in Louisville Kentucky. You can update your address in the Alexa app.";
                response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
                response.Response.ShouldEndSession = false;
                return response;
            }
                    
            if(!(await _zipCodeRepository.GetZipCodes()).Contains(zipCode))
            {
                response.Response.OutputSpeech.Type = "SSML";
                response.Response.OutputSpeech.Ssml = $"<speak>Your zip code, <say-as interpret-as=\"spell-out\">{zipCode}</say-as>, does not appear to be in Louisville Kentucky. Only addresses in Louisville Kentucky can be searched.</speak>";
                response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
                response.Response.ShouldEndSession = false;
                return response;
            }

            var address = addressApiResponse.Content.AddressLine1;
            var propertyInfo = await _pvaAddressLookup.GetPropertyInfo(address);

            if (!string.IsNullOrWhiteSpace(propertyInfo.Error))
            {
                response.Response.OutputSpeech.Text = $"Sorry, I wasn't able to find the owner of your address, {address}.";
                response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
                response.Response.ShouldEndSession = false;
                return response;
            }

            response.Response.OutputSpeech.Text = $"Your address, {address}, is owned by {propertyInfo.Owners}.";
            response.Response.Reprompt.OutputSpeech.Text = response.Response.OutputSpeech.Text;
            AddAddressCard(response, address, propertyInfo);
            response.Response.ShouldEndSession = false;

            return response;
        }

        private async Task<AlexaResponse> HandlePropertyValueIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                var addressSlot = request.Request.Intent.Slots["address"].ToObject<AddressSlot>();
                var address = addressSlot.Value;
                var propertyInfo = await _pvaAddressLookup.GetPropertyInfo(address);

                if (propertyInfo.AssessedValue == null)
                {
                    response.Response.OutputSpeech.Text = $"Sorry, I wasn't able to find the property valuation for {address}.";
                    response.Response.Reprompt.OutputSpeech.Text = $"Sorry, I wasn't able to find the property valuation for {address}.";
                    response.Response.ShouldEndSession = false;
                    return response;
                }

                response.Response.OutputSpeech.Text = $"The assessed value of {address} is {propertyInfo.AssessedValue} dollars.";
                response.Response.Reprompt.OutputSpeech.Text = $"The assessed value of {address} is {propertyInfo.AssessedValue} dollars.";
                AddAddressCard(response, address, propertyInfo);
                response.Response.ShouldEndSession = false;

                return response;
            }
            catch (Exception ex)
            {
                return AlexaSafeExceptionHandler.HandleException(_logger, ex, response);
            }
        }

        private async Task<AlexaResponse> HandleAcreageIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                var addressSlot = request.Request.Intent.Slots["address"].ToObject<AddressSlot>();
                var address = addressSlot.Value;
                var propertyInfo = await _pvaAddressLookup.GetPropertyInfo(address);

                if (propertyInfo.Acres == null)
                {
                    response.Response.OutputSpeech.Text = $"Sorry, I wasn't able to find the size of {address}.";
                    response.Response.Reprompt.OutputSpeech.Text = $"Sorry, I wasn't able to find the size of {address}.";
                    response.Response.ShouldEndSession = false;
                    return response;
                }

                response.Response.OutputSpeech.Text = $"The property at {address} is {propertyInfo.Acres} acres.";
                response.Response.Reprompt.OutputSpeech.Text = $"The property at {address} is {propertyInfo.Acres} acres.";
                AddAddressCard(response, address, propertyInfo);
                response.Response.ShouldEndSession = false;

                return response;
            }
            catch (Exception ex)
            {
                return AlexaSafeExceptionHandler.HandleException(_logger, ex, response);
            }
        }

        
        private async Task<AlexaResponse> HandleWhoLivesIntent(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                var responseText = "I can't tell you who lives at an address. I can only tell you who owns it.";

                var addressSlot = request.Request.Intent.Slots["address"].ToObject<AddressSlot>();
                var address = addressSlot.Value;
                var propertyInfo = await _pvaAddressLookup.GetPropertyInfo(address);

                if (!string.IsNullOrWhiteSpace(propertyInfo.Error))
                {
                    response.Response.OutputSpeech.Text = responseText;
                    response.Response.Reprompt.OutputSpeech.Text = responseText;
                    response.Response.ShouldEndSession = false;
                    return response;
                }

                response.Response.OutputSpeech.Text = $"{responseText} The owner of {address} is {propertyInfo.Owners}.";
                response.Response.Reprompt.OutputSpeech.Text = $"{responseText} The owner of {address} is {propertyInfo.Owners}.";
                AddAddressCard(response, address, propertyInfo);
                response.Response.ShouldEndSession = false;

                return response;
            }
            catch (Exception ex)
            {
                return AlexaSafeExceptionHandler.HandleException(_logger, ex, response);
            }
        }

        private void AddAddressCard(AlexaResponse response, string address, PvaPropertyInfo propertyInfo)
        {
            response.Response.Card.Type = "Standard";
            response.Response.Card.Title = address;
            response.Response.Card.Text = $"Owner: {propertyInfo.Owners}\r\nAssessed Value: ${string.Format("{0:n0}", propertyInfo.AssessedValue)}\r\nAcres: {propertyInfo.Acres}";
            response.Response.Card.Image.SmallImageUrl = _mapImageUrlGenerator.GetMapUrl(address, MapSize.Small);
            response.Response.Card.Image.LargeImageUrl = _mapImageUrlGenerator.GetMapUrl(address, MapSize.Large);
        }

        public async Task<AlexaResponse> HandleSessionEndedRequest(AlexaRequest request, AlexaResponse response)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return AlexaSafeExceptionHandler.HandleException(_logger, ex, response);
            }
        }
    }
}