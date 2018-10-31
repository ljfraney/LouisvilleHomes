using System;
using System.Threading.Tasks;

namespace TheVilleSkill.Utilities.Caching
{
    public interface ICacheAdapter
    {
        Task Add(string key, object value, TimeSpan cacheDuration);
        Task<T> Get<T>(string key);
        Task<string> GetString(string key);
        Task Remove(string key);
        Task SetString(string key, string value, TimeSpan cacheDuration);
    }
}