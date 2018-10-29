using System.Collections.Generic;

namespace LouisvilleHomes.Data
{
    public class Tag
    {
        public Tag()
        {
            Addresses = new HashSet<Address>();
        }

        public string USPSStandardAbbreviation { get; set; }
        public string Name { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<TagCommonAbbreviation> CommonAbbreviations { get; set; }
    }
}
