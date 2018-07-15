using System;
using BridgeportClaims.Data.DataProviders.ClaimImages;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.DataProviders.Episodes;
using BridgeportClaims.Data.DataProviders.Payments;
using BridgeportClaims.Data.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Testing.Specificity;

namespace BridgeportClaims.Integration.Tests.Claims
{
    [TestClass]
    public class ClaimsTests
    {
        private IClaimsDataProvider _claimsDataProvider;
        private readonly Mock<Lazy<IPaymentsDataProvider>> _paymentsDataProvider = new Mock<Lazy<IPaymentsDataProvider>>();
        private readonly Mock<Lazy<IClaimImageProvider>> _claimImageProvider = new Mock<Lazy<IClaimImageProvider>>();
        private readonly Mock<Lazy<IEpisodesDataProvider>> _episodeDataProvider = new Mock<Lazy<IEpisodesDataProvider>>();
        private const string Id = "6b36c7fb-dc13-4ed3-b83c-260bc8e42018";

        [TestInitialize]
        public void Initialize()
        {
            _claimsDataProvider = new ClaimsDataProvider(_paymentsDataProvider.Object, _claimImageProvider.Object,
                _episodeDataProvider.Object);
        }

        [TestMethod]
        public void GetClaimsDataTests()
        {
            // Arrange. Act.
            var operation = _claimsDataProvider.AddOrUpdateFlex2(775, 1, Id);

            // Assert.
            Specify.That(operation == EntityOperation.Add || operation == EntityOperation.Update).Should.BeTrue();
        }
    }
}
