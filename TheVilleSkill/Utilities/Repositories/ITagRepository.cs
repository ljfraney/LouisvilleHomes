using System.Collections.Generic;
using System.Threading.Tasks;
using TheVilleSkill.Models.Addresses;

namespace TheVilleSkill.Utilities.Repositories
{
    public interface ITagRepository
    {
        Task<List<TagModel>> GetTags();

        Task<List<TagAbbreviationModel>> GetCommonAbbreviations();
    }
}