using System.Threading.Tasks;
using TheVilleSkill.Models.Eventful.Response;

namespace TheVilleSkill.Utilities.HttpClients
{
    public interface IEventfulHttpClient
    {
        Task<EventSearchResponse> GetEvents();
    }
}