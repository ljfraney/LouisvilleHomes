using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheVilleSkill.Utilities.Repositories
{
    public interface IZipCodeRepository
    {
        Task<List<string>> GetZipCodes();
    }
}