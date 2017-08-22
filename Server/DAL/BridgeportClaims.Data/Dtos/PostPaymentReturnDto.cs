using System;
using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public sealed class PostPaymentReturnDto
    {
        public PostPaymentReturnDto()
        {
            PostPaymentPrescriptionReturnDtos = new List<PostPaymentPrescriptionReturnDto>();
        }
        public string ToastMessage { get; set; }
        public decimal AmountRemaining { get; set; }
        public IList<PostPaymentPrescriptionReturnDto> PostPaymentPrescriptionReturnDtos { get; set; }
    }
}
