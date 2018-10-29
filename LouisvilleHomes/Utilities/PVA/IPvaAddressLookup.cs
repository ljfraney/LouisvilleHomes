using LouisvilleHomes.Models.PVA;
using System.Threading.Tasks;

namespace LouisvilleHomes.Utilities.PVA
{
    public interface IPvaAddressLookup
    {
        Task<PvaPropertyInfo> GetPropertyInfo(string address);
    }
}