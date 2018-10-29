using LouisvilleHomes.Models.GoogleMaps;

namespace LouisvilleHomes.Utilities.GoogleMaps
{
    public interface IMapImageUrlGenerator
    {
        string GetMapUrl(string address, MapSize size);
    }
}