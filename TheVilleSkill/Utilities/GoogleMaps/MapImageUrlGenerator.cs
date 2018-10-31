using TheVilleSkill.Models.GoogleMaps;
using Microsoft.Extensions.Logging;
using System.Web;

namespace TheVilleSkill.Utilities.GoogleMaps
{
    public class MapImageUrlGenerator : IMapImageUrlGenerator
    {
        private ILogger<MapImageUrlGenerator> _logger;
        private GoogleMapsSettings _settings;

        public MapImageUrlGenerator(ILogger<MapImageUrlGenerator> logger, GoogleMapsSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public string GetMapUrl(string address, MapSize size)
        {
            var urlAddress = HttpUtility.UrlEncode(address);

            var width = size == MapSize.Small ? _settings.SmallWidth : _settings.LargeWidth;
            var height = size == MapSize.Small ? _settings.SmallHeight : _settings.LargeHeight;
            var zoom = size == MapSize.Small ? _settings.SmallZoom : _settings.LargeZoom;

            var url = $"{_settings.Url}?zoom={zoom}&size={width}x{height}&maptype={_settings.MapType}&markers=size:{_settings.MarkerSize}%7Ccolor:{_settings.MarkerColor}%7C{urlAddress}&key={_settings.ApiKey}";
            _logger.LogInformation("Map URL for {address}: {url}", address, url);
            return url;
        }
    }
}
