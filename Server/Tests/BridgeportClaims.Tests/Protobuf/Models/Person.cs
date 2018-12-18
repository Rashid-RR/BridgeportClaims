using System;
using ProtoBuf;

namespace BridgeportClaims.Tests.Protobuf.Models
{
    [ProtoContract(SkipConstructor = true)]
    public sealed class Person
    {
        [ProtoMember(1)]
        public string FirstName { get; set; }
        [ProtoMember(2)]
        public string LastName { get; set; }
        [ProtoMember(3)]
        public DateTime JoinDate { get; set; }
        public string NonProtoMemberProperty { get; set; }
    }
}