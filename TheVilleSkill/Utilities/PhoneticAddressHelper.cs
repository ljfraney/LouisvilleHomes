using System.Linq;
using System.Threading.Tasks;
using TheVilleSkill.Models.Addresses;
using TheVilleSkill.Utilities.Repositories;

namespace TheVilleSkill.Utilities
{
    public class PhoneticAddressHelper : IPhoneticAddressHelper
    {
        private ITagRepository _tagRepository;

        public PhoneticAddressHelper(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<string> GetPhoneticAddress(AddressModel address)
        {
            return $"{address.Number} {address.HalfHouse} {GetDirectionName(address.Direction)} {address.Street} {await GetTagName(address.Tag)} {(!string.IsNullOrWhiteSpace(address.Apartment) ? " Apartment " + address.Apartment : "")}";
        }

        public async Task<string> GetPhoneticBaseAddress(AddressModel address)
        {
            return $"{address.Number} {GetDirectionName(address.Direction)} {address.Street} {await GetTagName(address.Tag)}";
        }

        private async Task<string> GetTagName(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return "";

            var tagName = (await _tagRepository.GetTags()).FirstOrDefault(t => t.PostalAbbreviation == tag)?.Name;

            return tagName ?? tag;
        }

        private string GetDirectionName(string direction)
        {
            if (string.IsNullOrWhiteSpace(direction))
                return "";

            switch (direction.ToUpper())
            {
                case "N":
                case "NORTH":
                    return "North";
                case "S":
                case "SOUTH":
                    return "South";
                case "E":
                case "EAST":
                    return "East";
                case "W":
                case "WEST":
                    return "West";
                default:
                    return "";
            }
        }
    }
}
