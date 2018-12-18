using BridgeportClaims.Data.Dtos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BridgeportClaims.Tests.Dtos
{
    [TestClass]
    public class DtosTests
    {
        [TestMethod]
        public void EnsureDtoHasNeededField()
        {
            var item = new ClaimsWithPrescriptionDetailsDto();
            Assert.IsFalse(item.IsReversed);
        }
    }
}