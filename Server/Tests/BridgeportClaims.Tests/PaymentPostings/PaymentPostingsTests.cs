using BridgeportClaims.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BridgeportClaims.Tests.PaymentPostings
{
    [TestClass]
    public class PaymentPostingsTests
    {
        [TestMethod]
        public void TestingAutoIncrementIdOfPaymentPostingObjects()
        {
            // Arrange.
            var test1 = new PaymentPosting();
            var test2 = new PaymentPosting();
            var test3 = new PaymentPosting();

            // Act. The unit testing seems like it runs multiple threads 
            // and therefore the seed is unknown.
            var seed = test1.Id;

            // Act. Assert.
            var seedPlusOne = seed + 1;
            var seedPlusTwo = seed + 2;
            Assert.AreEqual(seedPlusOne, test2.Id);
            Assert.AreEqual(seedPlusTwo, test3.Id);

        }
    }
}