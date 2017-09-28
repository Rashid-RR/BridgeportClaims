using System;
using BridgeportClaims.Common.Caching;
using BridgeportClaims.Web.Models;
using Microsoft.AspNet.SignalR;

namespace BridgeportClaims.Web.Hubs
{
    public class PostingHub : Hub
    {
        private readonly IMemoryCacher _memoryCacher;
        private static readonly UserPaymentPostingSession Shell = null;

        public PostingHub()
        {
            _memoryCacher = MemoryCacher.Instance;
        }

        /// <summary>
        /// Assumes a UserPaymentPostingSession object has already been stored in Session and is now posting / broadcasting out via SignalR
        /// </summary>
        /// <param name="cacheKey"></param>
        public void PostedPayment(string cacheKey)
        {
            var model = _memoryCacher.AddOrGetExisting(cacheKey, () => Shell);
            if (null == model)
                throw new ArgumentNullException(nameof(model));
            Clients.Client(154.ToString()).postPayment(model); // TODO: Remove or replace. 154 is a hard-coded destination connection ID.
        }
    }
}