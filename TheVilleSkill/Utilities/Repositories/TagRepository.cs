using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheVilleSkill.Data;
using TheVilleSkill.Models.Addresses;
using TheVilleSkill.Models.Caching;
using TheVilleSkill.Utilities.Caching;

namespace TheVilleSkill.Utilities.Repositories
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

        public async Task<List<TagModel>> GetTags()
        {
            const string cacheKey = "TagList";
            var cachedTags = await _cacheAdapter.Get<List<TagModel>>(cacheKey);

            if (cachedTags != null)
                return cachedTags;

            var tags = _louisvilleDemographicsContext.Tags
                .Select(t => new TagModel {
                    PostalAbbreviation = t.USPSStandardAbbreviation,
                    Name = t.Name
                }).ToList();

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
