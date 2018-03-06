using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BridgeportClaims.Entities.DomainModels
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public class FileType
    {
        public FileType()
        {
            Document = new List<Document>();
        }
        [Required]
        public virtual byte FileTypeId { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string TypeName { get; set; }
        [Required]
        [StringLength(10)]
        public virtual string Code { get; set; }
        [Required]
        public virtual DateTime CreatedOnUtc { get; set; }
        [Required]
        public virtual DateTime UpdatedOnUtc { get; set; }
        public virtual IList<Document> Document { get; set; }
    }
}