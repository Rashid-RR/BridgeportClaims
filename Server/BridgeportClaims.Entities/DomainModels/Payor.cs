using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BridgeportClaims.Entities.Domain;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class Payor : IEntity
    {
        public Payor()
        {
            Adjustor = new List<Adjustor>();
            Claim = new List<Claim>();
        }
        public virtual int Id { get; set; }
        public virtual UsState UsState { get; set; }
        public virtual string BillToName { get; set; }
        public virtual string BillToAddress1 { get; set; }
        public virtual string BillToAddress2 { get; set; }
        public virtual string BillToCity { get; set; }
        public virtual string BillToPostalCode { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string AlternatePhoneNumber { get; set; }
        public virtual string FaxNumber { get; set; }
        public virtual string Notes { get; set; }
        public virtual string Contact { get; set; }
        [Required]
        public virtual DateTime CreatedOn { get; set; }
        [Required]
        public virtual DateTime UpdatedOn { get; set; }
        public virtual IList<Adjustor> Adjustor { get; set; }
        public virtual IList<Claim> Claim { get; set; }
    }
}
