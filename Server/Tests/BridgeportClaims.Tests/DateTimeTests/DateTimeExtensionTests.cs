using System;
using BridgeportClaims.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BridgeportClaims.Tests.DateTimeTests
{
    [TestClass]
    public class DateTimeExtensionTests
    {
        [TestMethod]
        public void TestBothDateTimeExtensionsForExactParsing()
        {
            // Arrange.
            const string date1 = "9/1/1908";
            const string date2 = "9/11/2002";
            const string date3 = "11/11/2012";
            const string date4 = "11/20/2012";
            const string date5 = "01/5/2002";
            const string date6 = "1/05/2022";
            const string date7 = "1/2/2002";
            const string date8 = "01/02/2002";

            // Act.
            var nullableVal = ((string) null).ToNullableFormattedDateTime();
            var date1Val = date1.ToFormattedDateTime();
            var date2Val = date2.ToFormattedDateTime();
            var date3Val = date3.ToFormattedDateTime();
            var date4Val = date4.ToFormattedDateTime();
            var date5Val = date5.ToFormattedDateTime();
            var date6Val = date6.ToFormattedDateTime();
            var date7Val = date7.ToFormattedDateTime();
            var date8Val = date8.ToFormattedDateTime();

            // Assert.
            Assert.IsNull(nullableVal);
            Assert.AreEqual(date1Val, new DateTime(1908, 9, 1));
            Assert.AreEqual(date2Val, new DateTime(2002, 9, 11));
            Assert.AreEqual(date3Val, new DateTime(2012, 11, 11));
            Assert.AreEqual(date4Val, new DateTime(2012, 11, 20));
            Assert.AreEqual(date5Val, new DateTime(2002, 1, 5));
            Assert.AreEqual(date6Val, new DateTime(2022, 1, 5));
            Assert.AreEqual(date7Val, new DateTime(2002, 1, 2));
            Assert.AreEqual(date8Val, new DateTime(2002, 1, 2));
        }
    }
}