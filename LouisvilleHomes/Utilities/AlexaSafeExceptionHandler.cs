using LouisvilleHomes.Models.Alexa;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.CompilerServices;

namespace LouisvilleHomes.Utilities
{
    public static class AlexaSafeExceptionHandler
    {
        public static AlexaResponse HandleException(ILogger logger, Exception exception, AlexaResponse response, [CallerMemberName] string methodName = "")
        {
            logger.LogCritical(exception, $"An unhandled error occurred in the {methodName}() method: {exception.ToString()}");

            response.Response.ShouldEndSession = true;
            response.Response.OutputSpeech.Text = "Oops. We encountered some trouble. Sorry about that. We'll look into this. Please try again later.";
            return response;
        }
    }
}