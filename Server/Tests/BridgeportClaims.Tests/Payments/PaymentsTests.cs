using System.Collections.Generic;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BridgeportClaims.Tests.Payments
{
    [TestClass]
    public class PaymentsTests
    {
        private IMemoryCacher _cache;
        private static readonly UserPaymentPostingSession Shell = null;

        [TestInitialize]
        public void Setup()
        {
            _cache = MemoryCacher.Instance; // Is this the difference maker?
            _cache.DeleteAll(); // Clear everything away for tests to run clean.
        }

        [TestCleanup]
        public void TearDown()
        {
            _cache.DeleteAll();
        }

        [TestMethod]
        public void TestWetherOrNotTheContainsMethodWorks()
        {
            // Arrange.
            const string key = "__MyUserPaymentPostingSessionCacheTestKey__";
            var model = new UserPaymentPostingSession
            {
                CheckAmount = 500,
                SuspenseAmountRemaining = 0
            };

            // Act / Assert.
            Assert.IsFalse(_cache.Contains(key)); // It cannot contain the key yet.
            var addOrGetExistingItem = _cache.AddOrGetExisting(key, () => model);
            Assert.IsTrue(_cache.Contains(key));
            var itemFromCache = _cache.AddOrGetExisting(key, () => model);
            Assert.AreEqual(itemFromCache, model);
            Assert.AreEqual(addOrGetExistingItem, model);
            _cache.Delete(key);
            _cache.Delete("blahalfh"); // Ensure this doesn't just blow up.
            Assert.IsFalse(_cache.Contains(key));
        }

        [TestMethod]
        public void AddOrGetExistingIsTheSameAsLongAsTheGenericObjecitItsReturningIsTheSame()
        {
            const string key = "__GetItemSameAsAddOrGetExistingKey__";
            _cache.DeleteAll();
            // Now that we've added it the proper way
            var model = _cache.AddOrGetExisting(key, GetFixedUserPaymentPostingSession);
            var cachedModel = _cache.AddOrGetExisting(key, () => Shell);
            Assert.AreEqual(cachedModel, model);
        }

        [TestMethod]
        public void TestUpdateingItem()
        {
            // Arrange.
            const string key = "__UpdateItemTestKey__";
            _cache.DeleteAll();
            var model = _cache.AddOrGetExisting(key, GetFixedUserPaymentPostingSession);

            // Act, change something.
            model.CheckAmount = 69.69m;
            _cache.UpdateItem(key, model);
            var updatedCachedItem = _cache.AddOrGetExisting(key, () => Shell);

            // Assert.
            Assert.AreEqual(updatedCachedItem, model);
            Assert.IsTrue(_cache.Contains(key));
        }

        private static UserPaymentPostingSession GetFixedUserPaymentPostingSession()
        {
            var model = new UserPaymentPostingSession
            {
                CheckAmount = 1000000,
                AmountSelected = 8848,
                CheckNumber = "858458",
                LastAmountRemaining = null,
                PaymentPostings = new List<PaymentPosting>
                {
                    new PaymentPosting
                    {
                        AmountPosted = 300,
                        CurrentOutstanding = 200,
                        PatientName = "RadCliff",
                        PrescriptionId = 4444
                    }
                }
            };
            return model;
        }

        [TestMethod]
        public void CachingTestForPaymentsCachingSystem()
        {
            const string firstKey = "__FirstKey__";
            const string secondKey = "__SecondKey__";
            var model = GetFixedUserPaymentPostingSession();
            var gettingBackWhatIJustCreated = _cache.AddOrGetExisting(firstKey, () => model);
            Assert.AreEqual(gettingBackWhatIJustCreated, model);
            var fakeModel = new UserPaymentPostingSession
            {
                CheckAmount = 8,
                AmountSelected = 6,
                CheckNumber = "21",
                LastAmountRemaining = null,
                PaymentPostings = null
            };
            // what I pass in means absolutely nothing is an object is already there.
            var instance1 = _cache.AddOrGetExisting(firstKey, GenerateNewInstanceOfSameObject);
            // These will not be equal if the <T> in the AddOrGetExisting changes.
            Assert.AreNotEqual(instance1, model);
            var instance2 = _cache.AddOrGetExisting(firstKey, GenerateNewInstanceOfSameObject);
            // These are not equal because they're two separate instances.
            Assert.AreNotEqual(instance1, instance2);
            // We're going to get back our first model.
            var thisWontBeMyFakeModel = _cache.AddOrGetExisting(firstKey, () => fakeModel);
            Assert.AreNotEqual(fakeModel, thisWontBeMyFakeModel);
            Assert.AreEqual(thisWontBeMyFakeModel, model);

            // But.. if I clear the cache, then fill it with my fake model, that's what I'll get back
            _cache.Delete(firstKey);
            var nowThisWillBeMyFakeModel = _cache.AddOrGetExisting(firstKey, () => fakeModel);
            Assert.AreEqual(nowThisWillBeMyFakeModel, fakeModel);
            // This won't matter that I'm passing in a different version of the model, I'll stil get
            // back fakeModel
            var modelWithFirstKey = _cache.AddOrGetExisting(firstKey, () => model);
            Assert.AreEqual(modelWithFirstKey, fakeModel);
            // Reset, new Test
            _cache.DeleteAll();
            var anotherModelWithFirstKey = _cache.AddOrGetExisting(firstKey, () => model);
            var anotherModelWithSecondKey = _cache.AddOrGetExisting(secondKey, () => model);
            Assert.AreEqual(anotherModelWithFirstKey, anotherModelWithSecondKey);
            _cache.Delete(firstKey);
            Assert.IsFalse(_cache.Contains(firstKey));
            Assert.IsTrue(_cache.Contains(secondKey));
            _cache.DeleteAll();
            Assert.IsFalse(_cache.Contains(secondKey));
        }

        private static SaveEpisodeModel GenerateNewInstanceOfSameObject()
        {
            return new SaveEpisodeModel();
        }


    }
}