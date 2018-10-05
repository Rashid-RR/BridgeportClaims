using System;
using BridgeportClaims.Common.Protobuf;
using BridgeportClaims.Tests.Assertions;
using BridgeportClaims.Tests.Protobuf.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BridgeportClaims.Tests.Protobuf
{
    [TestClass]
    public class ProtobufTests
    {
        private string _firstName;
        private string _lastName;
        private DateTime _joinDate;
        private string _nonProtoMemberProperty;
        private Person person;


        [TestInitialize]
        public void Initialize()
        {
            _firstName = "Kevin";
            _lastName = "Durant";
            _joinDate = new DateTime(2018, 2, 1);
            _nonProtoMemberProperty = "{NULL}";
            person = new Person
                { FirstName = _firstName, LastName = _lastName, JoinDate = _joinDate };
        }

        [TestMethod]
        public void ObjectSerializationOfANonProtoMemberProperty()
        {
            var bridgeportAssert = new BridgeportAssert();
            Assert.IsTrue(
                bridgeportAssert.AssertThrows<AssertFailedException>(() =>
                {
                    var objectForStorage = ProtobufService.ProtoSerialize(person);
                    var deserializedPerson = ProtobufService.ProtoDeserialize<Person>(objectForStorage);
                    Assert.AreEqual(deserializedPerson.NonProtoMemberProperty, _nonProtoMemberProperty);
                })
            );
        }

        [TestMethod]
        public void ObjectCanSerializeAndDeserialize()
        {
            // Arrange the Person class - Done.
            // Act.
            var objectForStorage = ProtobufService.ProtoSerialize(person);

            // Assert.
            Assert.IsNotNull(objectForStorage);

            // Act.
            var deserializedPerson = ProtobufService.ProtoDeserialize<Person>(objectForStorage);

            // Assert.
            Assert.AreEqual(_firstName, deserializedPerson.FirstName);
            Assert.AreEqual(_lastName, deserializedPerson.LastName);
            Assert.AreEqual(_joinDate, deserializedPerson.JoinDate);
            Assert.AreNotEqual(_joinDate, deserializedPerson.JoinDate.AddDays(1));
        }
    }
}
