using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheVilleSkill.Data
{
    public class AddressAttribute
    {
        public int Id { get; set; }

        public int AddressId { get; set; }

        public string Source { get; set; }

        public DateTime VerificationTime { get; set; }

        public string AttributeName { get; set; }

        public string AttributeValue { get; set; }

        public Address Address { get; set; }
    }
}
