using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class PostPaymentReturnDto
    {
        public PostPaymentReturnDto()
        {
            PostPaymentPrescriptionReturnDtos = new List<PostPaymentPrescriptionReturnDto>();
        }
        public string ToastMessage { get; set; }
        public decimal AmountRemaining { get; set; }
        public List<PostPaymentPrescriptionReturnDto> PostPaymentPrescriptionReturnDtos { get; set; }
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
