using System;
using BridgeportClaims.Business.Models;
using BridgeportClaims.Common.Caching;

namespace BridgeportClaims.Business.Payments
{
    public class PaymentsBusiness : IPaymentsBusiness
    {
        private readonly IMemoryCacher _cache;

        public PaymentsBusiness() => _cache = MemoryCacher.Instance;

        public bool CheckMultiLinePartialPayments(decimal amountSelected, decimal amountToPost,
            int countOfPrescriptions) => amountSelected == amountToPost || countOfPrescriptions <= 1;

        public decimal PostPartialPayment(string userId, PaymentInputsModel model)
        {
            if (null == model)
                throw new ArgumentNullException(nameof(model));
            return _cache.AddOrGetExisting(userId, () =>
            {
                var amountRemaining = model.CheckAmount - model.AmountToPost;
                return amountRemaining;
            });
        }
    }
}