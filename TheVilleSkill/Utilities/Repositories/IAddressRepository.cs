using System.Collections.Generic;
using System.Threading.Tasks;
using TheVilleSkill.Models.Addresses;

namespace TheVilleSkill.Utilities.Repositories
{
    public interface IAddressRepository
    {
        Task<AddressModel> Get(int number, string direction, string street, string tag);

        Task<int> Add(int number, string direction, string street, string tag);

        Task AddAttributes(int id, IEnumerable<AddressAttributeModel> attributes);
    }
}