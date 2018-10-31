using TheVilleSkill.Models.GoogleMaps;

namespace TheVilleSkill.Utilities.GoogleMaps
{
    public interface IMapImageUrlGenerator
    {
        string GetMapUrl(string address, MapSize size);
    }
}