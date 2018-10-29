using LouisvilleHomes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LouisvilleHomes.Utilities.Repositories
{
    public interface ITagRepository
    {
        Task<List<string>> GetTags();

        Task<List<TagAbbreviationModel>> GetCommonAbbreviations();
    }
}