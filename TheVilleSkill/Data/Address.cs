using System.Collections.Generic;

namespace TheVilleSkill.Data
{
    public partial class Address
    {
        public Address()
        {
            Attributes = new HashSet<AddressAttribute>();
        }

        public int Id { get; set; }

        public int Number { get; set; }

        public string Direction { get; set; }

        public string Street { get; set; }

        public string Tag { get; set; }

        public string Zip { get; set; }

        public Tag Tag1 { get; set; }

        public ICollection<AddressAttribute> Attributes { get; set; }
    }
}
