using System;

namespace BridgeportClaims.Tests.Protobuf.Models
{
    public static class PersonBuilder
    {
        public static Person BuildPerson()
        {
            const string firstName = "Kevin";
            const string lastName = "Durant";
            var joinDate = new DateTime(2018, 2, 1);
            const string nonProtoMemberProperty = "{NULL}";
            return new Person
            {
                FirstName = firstName, LastName = lastName, JoinDate = joinDate,
                NonProtoMemberProperty = nonProtoMemberProperty
            };
        }
    }
}