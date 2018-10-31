using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Threading.Tasks;

namespace TheVilleSkill.Utilities.Caching
{
    public class NoCacheDistributedCache : IDistributedCache
    {
        public byte[] Get(string key) => new byte[0];

        public Task<byte[]> GetAsync(string key, CancellationToken token = default(CancellationToken)) => Task.FromResult<byte[]>(null);

        public void Refresh(string key)
        {
            // Do nothing
        }

        public Task RefreshAsync(string key, CancellationToken token = default(CancellationToken)) => Task.CompletedTask;

        public void Remove(string key)
        {
            // Do nothing
        }

        public Task RemoveAsync(string key, CancellationToken token = default(CancellationToken)) => Task.CompletedTask;

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            // Do nothing
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)) => Task.CompletedTask;
    }
}
