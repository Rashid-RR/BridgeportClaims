using System;
using System.Threading;

namespace BridgeportClaims.Web.Models
{
    public class PaymentPosting
    {
        public PaymentPosting()
        {
            Id = Interlocked.Increment(ref _nextId);
        }
        private static int _nextId;
        public int Id { get; }
        public int DecrementId() => Interlocked.Decrement(ref _nextId);
        public string PatientName { get; set; }
        public DateTime RxDate { get; set; }
        public decimal AmountPosted { get; set; }
        public decimal CurrentOutstanding { get; set; }
        public decimal Outstanding => CurrentOutstanding - AmountPosted;
        public int PrescriptionId { get; set; }
    }
}