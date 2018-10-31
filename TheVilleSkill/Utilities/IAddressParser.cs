using System.Collections.Generic;
using System.Threading.Tasks;
using TheVilleSkill.Models.Addresses;

namespace TheVilleSkill.Utilities
{
    public interface IAddressParser
    {
        Task<bool> AreEqual(string firstAddress, string secondAddress);

        Task<IEnumerable<AddressResult>> Parse(string address1);
    }
}