using Microsoft.Extensions.Caching.Distributed;
using ProtoBuf;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TheVilleSkill.Utilities.Caching
{
    public class CacheAdapter : ICacheAdapter
    {
        private readonly IDistributedCache _cache;

        public CacheAdapter(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> Get<T>(string key)
        {
            var byteArray = await _cache.GetAsync(key);
            if (byteArray == null)
                return default(T);

            return ByteArrayToObject<T>(byteArray);
        }

        public async Task<string> GetString(string key) => await _cache.GetStringAsync(key);

        public async Task Add(string key, object value, TimeSpan cacheDuration)
        {
            var cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.SetAbsoluteExpiration(cacheDuration);

            await _cache.SetAsync(key, ObjectToByteArray(value), cacheOptions);
        }

        public async Task SetString(string key, string value, TimeSpan cacheDuration)
        {
            var distributedCacheEntryOptions = new DistributedCacheEntryOptions();
            distributedCacheEntryOptions.SetAbsoluteExpiration(cacheDuration);

            await _cache.SetStringAsync(key, value, distributedCacheEntryOptions);
        }

        public Task Remove(string key) => _cache.RemoveAsync(key);

        private byte[] ObjectToByteArray<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private T ByteArrayToObject<T>(byte[] arrBytes)
        {
            using (var ms = new MemoryStream(arrBytes))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}
