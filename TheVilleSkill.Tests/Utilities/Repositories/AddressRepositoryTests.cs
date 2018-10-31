using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheVilleSkill.Data;
using TheVilleSkill.Models.PVA;
using TheVilleSkill.Utilities;
using TheVilleSkill.Utilities.Repositories;
using Xunit;

namespace TheVilleSkill.Tests
{
    public class AddressRepositoryTests
    {
        [Fact]
        public async Task Get_ReturnsNonExpiredAttributes()
        {
            const string nowString = "10/31/2018 10:54:30";
            const int refreshAfterDays = 10;

            var address = new Address
            {
                Number = 123,
                Street = "MAIN",
                Tag = "ST",
                Attributes = new List<AddressAttribute>
                {
                    // 11 days old. Expired.
                    new AddressAttribute { Source = "PVA", VerificationTime = DateTime.Parse("10/20/2018 10:00:00"), AttributeName = "owner", AttributeValue = "JOHN DOE" },
                    // 10 days old. Expired.
                    new AddressAttribute { Source = "PVA", VerificationTime = DateTime.Parse("10/21/2018 10:00:00"), AttributeName = "owner", AttributeValue = "JANE DOE" },
                    // 9 days old, but not the newest.
                    new AddressAttribute { Source = "PVA", VerificationTime = DateTime.Parse("10/22/2018 10:00:00"), AttributeName = "owner", AttributeValue = "SAM DOE" },
                    // 9 days old, should be used.
                    new AddressAttribute { Source = "PVA", VerificationTime = DateTime.Parse("10/22/2018 10:00:01"), AttributeName = "owner", AttributeValue = "JUDY DOE"}
                }
            };

            var options = new DbContextOptionsBuilder<LouisvilleDemographicsContext>()
                .UseInMemoryDatabase(databaseName: "Test_Address_Attribute_Expiration")
                .Options;

            var louisvilleDemographicsContext = new LouisvilleDemographicsContext(options);
            louisvilleDemographicsContext.Addresses.Add(address);
            louisvilleDemographicsContext.SaveChanges();

            var pvaSettings = new PvaSettings { RefreshAfterDays = refreshAfterDays };

            var timeService = new Mock<ITimeService>();
            timeService.SetupGet(t => t.Now).Returns(DateTime.Parse(nowString));

            var addressRepository = new AddressRepository(null, louisvilleDemographicsContext, pvaSettings, timeService.Object);

            var result = await addressRepository.Get(address.Number, address.Direction, address.Street, address.Tag);

            Assert.Equal("JUDY DOE", result.Owner);
        }
    }
}