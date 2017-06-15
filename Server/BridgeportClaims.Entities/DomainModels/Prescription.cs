using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Prescription
    {
        public virtual int Id { get; set; }
        public virtual Claim Claim { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string RxNumber { get; set; }
        [Required]
        public virtual DateTime DateSubmitted { get; set; }
        [Required]
        public virtual DateTime DateFilled { get; set; }
        [StringLength(25)]
        public virtual string LabelName { get; set; }
        [Required]
        [StringLength(11)]
        public virtual string NDC { get; set; }
        [Required]
        public virtual float Quantity { get; set; }
        [Required]
        public virtual float DaySupply { get; set; }
        [Required]
        [StringLength(1)]
        public virtual string Generic { get; set; }
        [Required]
        [StringLength(7)]
        public virtual string PharmacyNABP { get; set; }
        public virtual float? AWP { get; set; }
        public virtual float? AWPUnit { get; set; }
        public virtual decimal? Usual { get; set; }
        [Required]
        [StringLength(10)]
        public virtual string Prescriber { get; set; }
        [Required]
        public virtual decimal PayableAmount { get; set; }
        [Required]
        public virtual decimal BilledAmount { get; set; }
        [Required]
        [StringLength(1)]
        public virtual string TransactionType { get; set; }
        [Required]
        [StringLength(1)]
        public virtual string Compound { get; set; }
        [Required]
        [StringLength(14)]
        public virtual string Tran { get; set; }
        public virtual DateTime? RefillDate { get; set; }
        public virtual short? RefillNumber { get; set; }
        [StringLength(1)]
        public virtual string MONY { get; set; }
        public virtual short? DAW { get; set; }
        [StringLength(14)]
        public virtual string GPI { get; set; }
        public virtual float? BillIngrCost { get; set; }
        public virtual float? BillDispFee { get; set; }
        public virtual float? BilledTax { get; set; }
        public virtual float? BilledCopay { get; set; }
        public virtual float? PayIngrCost { get; set; }
        public virtual float? PayDispFee { get; set; }
        public virtual float? PayTax { get; set; }
        [StringLength(12)]
        public virtual string DEA { get; set; }
        [StringLength(12)]
        public virtual string PrescriberNPI { get; set; }
        [StringLength(255)]
        public virtual string Strength { get; set; }
        [StringLength(255)]
        public virtual string GPIGenName { get; set; }
        [StringLength(255)]
        public virtual string TheraClass { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
        [Required]
        public virtual DateTime DataVersion { get; set; }
    }
}
