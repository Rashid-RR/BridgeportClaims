using System;
using System.Net;
using System.Web.Http;
using System.Threading;
using BridgeportClaims.Business.Payments;
using Microsoft.AspNet.SignalR;
using BridgeportClaims.Web.SignalR;
using Microsoft.AspNet.Identity;
using NLog;
using cs = BridgeportClaims.Common.Config.ConfigService;
using c = BridgeportClaims.Common.StringConstants.Constants;
using BridgeportClaims.Business.Models;
using BridgeportClaims.Web.Hubs;

namespace BridgeportClaims.Web.Controllers
{
    [System.Web.Http.Authorize(Roles = "Admin")]
    [RoutePrefix("api/new-features")]
    public class NewFeaturesController : ApiController
    {
        private IHubContext _context;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPaymentsBusiness _payments;

        public NewFeaturesController(IPaymentsBusiness payments)
        {
            _payments = payments;
            // Normally we would inject this
            _context = GlobalHost.ConnectionManager.GetHubContext<PaymentHub>();
        }

        [HttpPost]
        [Route("get-amount-remaining")]
        public IHttpActionResult GetAmountRemaining(PaymentInputsModel model)
        {
            try
            {
                if (null == model)
                    throw new ArgumentNullException(nameof(model));
                // Test SignalR
                if (cs.AppIsInDebugMode)
                    Logger.Info("App is in Debug Mode. Capturing SignalR interactions.");
                var random = new Random();
                var firstBits = random.Next(0, 1 << 4) << 28;
                var lastBits = random.Next(0, 1 << 28);
                model.CheckAmount = firstBits | lastBits;
                var remainingAmount = _payments.PostPartialPayment(User.Identity.GetUserId(), model);
                Logger.Info($"Testing Prod... The remaining amount is {remainingAmount}"); // TODO: remove after test.
                var status = new Status
                {
                    State = "starting",
                    PercentComplete = 0.0
                };

                PublishEvent("It's my time now!", status);
                const int steps = 5;
                for (var i = 0; i < steps; i++)
                {
                    // Update the status and publish a new event
                    //
                    status.State = "working in for loop";
                    status.PercentComplete = i / steps * 100;
                    PublishEvent("In for loop", status);
                    Thread.Sleep(500);
                }

                status.State = "It's done!";
                status.PercentComplete = 100;
                PublishEvent("Out of everything. Exiting...", status);
                return Ok(new { amountRemaining = model.CheckAmount });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Content(HttpStatusCode.NotAcceptable, new { message = ex.Message });
            }
        }

        private void PublishEvent(string eventName, Status status)
        {
            // From .NET code like this we can't invoke the methods that
            //  exist on our actual Hub class...because we only have a proxy
            //  to it. So to publish the event we need to call the method that
            //  the clients will be listening on.
            //
            _context.Clients.Group(c.PaymentChannel).OnEvent(c.PaymentChannel, new ChannelEvent
            {
                ChannelName = c.PaymentChannel,
                Name = eventName,
                Data = status
            });
        }
    }

    public class Status
    {
        public string State { get; set; }
        public double PercentComplete { get; set; }
    }
}
