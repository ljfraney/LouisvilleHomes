using System.Threading.Tasks;
using TheVilleSkill.Models.Addresses;

namespace TheVilleSkill.Utilities
{
    public interface IPhoneticAddressHelper
    {
        Task<string> GetPhoneticAddress(AddressModel address);
        Task<string> GetPhoneticBaseAddress(AddressModel address);
    }
}