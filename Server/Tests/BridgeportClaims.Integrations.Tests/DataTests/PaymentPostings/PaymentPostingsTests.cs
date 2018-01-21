using BridgeportClaims.Data.DataProviders.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BridgeportClaims.Integrations.Tests.DataTests.PaymentPostings
{
    [TestClass]
    public sealed class PaymentPostingsTests
    {
        [TestMethod]
        public void TestPaymentPosting()
        {
            IUtilitiesProvider provider = new UtilitiesProvider();
            var ppSeedValue = provider.ReseedTableAndGetSeedValue("dbo.PrescriptionPayment");
            Assert.IsNotNull(ppSeedValue);
        }
    }
}