using TheVilleSkill.Models.PVA;
using System.Threading.Tasks;

namespace TheVilleSkill.Utilities.PVA
{
    public interface IPvaAddressLookup
    {
        Task<PvaPropertyInfo> GetPropertyInfo(string address);
    }
}