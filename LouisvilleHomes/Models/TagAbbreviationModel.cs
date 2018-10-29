using ProtoBuf;

namespace LouisvilleHomes.Models
{
    [ProtoContract]
    public class TagAbbreviationModel
    {
        [ProtoMember(1)]
        public string StandardAbbreviation { get; set; }

        [ProtoMember(2)]
        public string CommonAbbreviation { get; set; }
    }
}
