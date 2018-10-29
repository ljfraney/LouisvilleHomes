using LouisvilleHomes.Data;
using LouisvilleHomes.Models;
using LouisvilleHomes.Models.Caching;
using LouisvilleHomes.Utilities.Caching;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LouisvilleHomes.Utilities.Repositories
{
    public class TagRepository : ITagRepository
    {
        private ILogger<TagRepository> _logger;
        private LouisvilleDemographicsContext _louisvilleDemographicsContext;
        private ICacheAdapter _cacheAdapter;
        private CacheSettings _cacheSettings;

        public TagRepository(ILogger<TagRepository> logger, LouisvilleDemographicsContext louisvilleDemographicsContext, ICacheAdapter cacheAdapter, CacheSettings cacheSettings)
        {
            _logger = logger;
            _louisvilleDemographicsContext = louisvilleDemographicsContext;
            _cacheAdapter = cacheAdapter;
            _cacheSettings = cacheSettings;
        }

        public async Task<List<string>> GetTags()
        {
            const string cacheKey = "TagList";
            var cachedTags = await _cacheAdapter.Get<List<string>>(cacheKey);

            if (cachedTags != null)
                return cachedTags;

            var tags = _louisvilleDemographicsContext.Tags.Select(t => t.USPSStandardAbbreviation).ToList();
            await _cacheAdapter.Add(cacheKey, tags, TimeSpan.FromMinutes(_cacheSettings.CacheTagsMinutes));

            return tags;
        }

        public async Task<List<TagAbbreviationModel>> GetCommonAbbreviations()
        {
            const string cacheKey = "TagCommonAbbreviationList";
            var cachedAbbreviations = await _cacheAdapter.Get<List<TagAbbreviationModel>>(cacheKey);

            if (cachedAbbreviations != null)
                return cachedAbbreviations;

            var abbreviations = _louisvilleDemographicsContext.TagCommonAbbreviations
                .Select(tca => new TagAbbreviationModel {
                    StandardAbbreviation = tca.USPSStandardAbbreviation,
                    CommonAbbreviation = tca.CommonAbbreviation
                }).ToList();
            await _cacheAdapter.Add(cacheKey, abbreviations, TimeSpan.FromMinutes(_cacheSettings.CacheTagsMinutes));

            return abbreviations;
        }
    }
}
