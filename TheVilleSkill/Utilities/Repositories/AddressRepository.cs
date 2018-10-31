using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheVilleSkill.Data;
using TheVilleSkill.Models.Addresses;
using TheVilleSkill.Models.PVA;

namespace TheVilleSkill.Utilities.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private ILogger<AddressRepository> _logger;
        private LouisvilleDemographicsContext _louisvilleDemographicsContext;
        private PvaSettings _pvaSettings;
        private ITimeService _timeService;

        public AddressRepository(ILogger<AddressRepository> logger,
            LouisvilleDemographicsContext louisvilleDemographicsContext, 
            PvaSettings pvaSettings, 
            ITimeService timeService)
        {
            _logger = logger;
            _louisvilleDemographicsContext = louisvilleDemographicsContext;
            _pvaSettings = pvaSettings;
            _timeService = timeService;
        }

        public async Task<AddressModel> Get(int number, string direction, string street, string tag)
        {
            direction = string.IsNullOrWhiteSpace(direction) ? null : direction.Trim();
            street = street.Trim();
            tag = string.IsNullOrWhiteSpace(tag) ? null : tag.Trim();

            if (string.IsNullOrWhiteSpace(tag))
                tag = null;

            var address = await _louisvilleDemographicsContext.Addresses
                .Include(a => a.Attributes)
                .FirstOrDefaultAsync(a => a.Number == number && a.Direction == direction && a.Street == street && a.Tag == tag);

            if (address == null)
                return null;

            var pvaAttributes = address.Attributes.Where(a => a.Source == "PVA" && (_timeService.Now - a.VerificationTime).TotalDays < _pvaSettings.RefreshAfterDays).OrderByDescending(a => a.VerificationTime);

            var owner = pvaAttributes.FirstOrDefault(a => a.AttributeName == "owner")?.AttributeValue;

            int? assessedValue = null;
            string strAssessedValue = pvaAttributes.FirstOrDefault(a => a.AttributeName == "assessedvalue")?.AttributeValue;
            if (!string.IsNullOrEmpty(strAssessedValue))
                assessedValue = int.Parse(strAssessedValue);

            double? acreage = null;
            string strAcreage = pvaAttributes.FirstOrDefault(a => a.AttributeName == "acreage")?.AttributeValue;
            if (!string.IsNullOrEmpty(strAcreage))
                acreage = double.Parse(strAcreage);

            var addressModel = new AddressModel
            {
                Id = address.Id,
                Number = address.Number,
                Direction = address.Direction,
                Street = address.Street,
                Tag = address.Tag,
                Owner = owner,
                AssessedValue = assessedValue,
                Acreage = acreage
            };

            return addressModel;
        }

        public async Task<int> Add(int number, string direction, string street, string tag)
        {
            direction = string.IsNullOrWhiteSpace(direction) ? null : direction.Trim();
            street = street.Trim();
            tag = string.IsNullOrWhiteSpace(tag) ? null : tag.Trim();

            var existingAddress = await Get(number, direction, street, tag);

            if (existingAddress != null)
                throw new AddressAlreadyExistsException(existingAddress);

            var address = new Address { Number = number, Direction = direction, Street = street, Tag = tag };

            _louisvilleDemographicsContext.Addresses.Add(address);

            await _louisvilleDemographicsContext.SaveChangesAsync();

            return address.Id;
        }

        public async Task AddAttributes(int id, IEnumerable<AddressAttributeModel> attributes)
        {
            var address = _louisvilleDemographicsContext.Addresses.SingleOrDefault(a => a.Id == id);

            if (address == null)
                throw new AddressDoesNotExistException(id);

            foreach (var attribute in attributes)
            {
                var addressAttribute = new AddressAttribute
                {
                    Source = attribute.Source,
                    AttributeName = attribute.Name,
                    AttributeValue = attribute.Value,
                    VerificationTime = _timeService.Now
                };

                address.Attributes.Add(addressAttribute);
            }

            await _louisvilleDemographicsContext.SaveChangesAsync();
        }
    }
}

public class AddressAlreadyExistsException : Exception
{
    public AddressAlreadyExistsException(AddressModel address) : base($"The address {address.AddressDisplay} already exists in the database.") { }
}

public class AddressDoesNotExistException : Exception
{
    public AddressDoesNotExistException(int addressId) : base($"The address with id {addressId} does not exist in the database.") { }
}
