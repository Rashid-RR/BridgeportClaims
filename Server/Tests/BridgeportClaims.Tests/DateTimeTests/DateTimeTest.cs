using System;
using BridgeportClaims.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BridgeportClaims.Tests.DateTimeTests
{
    [TestClass]
    public class DateTimeTest
    {
        [TestMethod]
        public void ToNullableDateTimeWorksWithNullAndDate()
        {
            // Arrange.
            const string date1 = "01/10/2018";
            const string date2 = "01/20/2018";
            
            // Act.
            var dt = date1.ToNullableFormattedDateTime();
            var ndt = date2.ToNullableFormattedDateTime();

            // Assert.
            Assert.IsInstanceOfType(dt, typeof(DateTime?));
            Assert.IsNotInstanceOfType(dt, typeof(DateTime));
            Assert.IsInstanceOfType(ndt, typeof(DateTime?));
            Assert.IsNotInstanceOfType(ndt, typeof(DateTime));
            Assert.AreEqual(dt, new DateTime(2018, 1, 10));
            Assert.AreEqual(ndt, new DateTime(2018, 1, 20));
        }
    }
}