using System;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.SignalR;

namespace BridgeportClaims.Web.Hubs
{
    public class PostingHub : Hub
    {
        private readonly IMemoryCacher _memoryCacher;

        public PostingHub(IMemoryCacher memoryCacher)
        {
            _memoryCacher = memoryCacher;
        }

        /// <summary>
        /// Assumes a UserPaymentPostingSession object has already been stored in Session and is now posting / broadcasting out via SignalR
        /// </summary>
        /// <param name="cacheKey"></param>
        public void PostedPayment(string cacheKey)
        {
            var model = _memoryCacher.GetItem(cacheKey) as UserPaymentPostingSession;
            if (null == model)
                throw new ArgumentNullException(nameof(model));
            Clients.Client(model.DestinationConnectionId).postPayment(model);
        }
    }
}