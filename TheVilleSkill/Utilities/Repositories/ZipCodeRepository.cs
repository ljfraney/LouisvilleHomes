using TheVilleSkill.Data;
using TheVilleSkill.Models.Caching;
using TheVilleSkill.Utilities.Caching;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheVilleSkill.Utilities.Repositories
{
    public class ZipCodeRepository : IZipCodeRepository
    {
        private ILogger<ZipCodeRepository> _logger;
        private LouisvilleDemographicsContext _louisvilleDemographicsContext;
        private ICacheAdapter _cacheAdapter;
        private CacheSettings _cacheSettings;

        public ZipCodeRepository(ILogger<ZipCodeRepository> logger,
            LouisvilleDemographicsContext louisvilleDemographicsContext,
            ICacheAdapter cacheAdapter,
            CacheSettings cacheSettings)
        {
            _logger = logger;
            _louisvilleDemographicsContext = louisvilleDemographicsContext;
            _cacheAdapter = cacheAdapter;
            _cacheSettings = cacheSettings;
        }

        public async Task<List<string>> GetZipCodes()
        {
            const string cacheKey = "ZipCodeList";
            var cachedZipCodes = await _cacheAdapter.Get<List<string>>(cacheKey);

            if (cachedZipCodes != null)
                return cachedZipCodes;

            var zipCodes = _louisvilleDemographicsContext.Addresses
                .Where(a => a.Zip != null)
                .Select(a => a.Zip)
                .Distinct().ToList();

            await _cacheAdapter.Add(cacheKey, zipCodes, TimeSpan.FromMinutes(_cacheSettings.CacheTagsMinutes));

            return zipCodes;
        }
    }
}
