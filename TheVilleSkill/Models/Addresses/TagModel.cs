using ProtoBuf;

namespace TheVilleSkill.Models.Addresses
{
    [ProtoContract]
    public class TagModel
    {
        [ProtoMember(1)]
        public string PostalAbbreviation { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }
    }
}
