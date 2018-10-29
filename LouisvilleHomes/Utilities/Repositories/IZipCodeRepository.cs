using System.Collections.Generic;
using System.Threading.Tasks;

namespace LouisvilleHomes.Utilities.Repositories
{
    public interface IZipCodeRepository
    {
        Task<List<string>> GetZipCodes();
    }
}