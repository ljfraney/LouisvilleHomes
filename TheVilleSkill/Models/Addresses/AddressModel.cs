namespace TheVilleSkill.Models.Addresses
{
    public class AddressModel
    {
        public string AddressDisplay => $"{Number}{HalfHouse}{(!string.IsNullOrWhiteSpace(Direction) ? " " + Direction : "")} {Street}{(!string.IsNullOrWhiteSpace(Tag) ? " " + Tag : "")}{(!string.IsNullOrWhiteSpace(Apartment) ? " APT " + Apartment : "")}";

        public string BaseAddressDisplay => $"{Number}{(!string.IsNullOrWhiteSpace(Direction) ? " " + Direction : "")} {Street}{(!string.IsNullOrWhiteSpace(Tag) ? " " + Tag : "")}";

        public int? Id { get; set; }

        public int Number { get; set; }

        public string HalfHouse { get; set; }

        public string Direction { get; set; }

        public string Street { get; set; }

        public string Tag { get; set; }

        public string Apartment { get; set; }

        public string Owner { get; set; }

        public int? AssessedValue { get; set; }

        public double? Acreage { get; set; }
    }
}
