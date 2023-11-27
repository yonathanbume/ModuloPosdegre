using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    [Table("VGroupFile")]
    public class VGroupFile : Entity, ITimestamp
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("VGroup")]
        public Guid VGroupId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [StringLength(250)]
        public string FileUrl { get; set; }

        public string NameFile { get; set; }

        public string Description { get; set; }
        public string UserName { get; set; }

        public DateTime DateTime { get; set; }
        public string FileName { get; set; }

        public virtual VGroup VGroup { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
