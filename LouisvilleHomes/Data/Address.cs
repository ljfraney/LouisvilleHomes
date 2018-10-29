namespace LouisvilleHomes.Data
{
    public partial class Address
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Direction { get; set; }
        public string Street { get; set; }
        public string Tag { get; set; }
        public string Zip { get; set; }

        public Tag Tag1 { get; set; }
    }
}
