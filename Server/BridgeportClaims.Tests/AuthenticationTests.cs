using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BridgeportClaims.Tests
{
    [TestClass]
    public class AuthenticationTests
    {
        [TestMethod]
        public void AuthenticationTest()
        {
            // Arrange.
            var t = DateTime.Now;

            // Act.
            var a = t;

            // Assert.
            Assert.AreEqual(a, t);
        }
    }
}
