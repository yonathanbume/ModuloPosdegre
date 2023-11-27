using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    [Table("VGroupUser")]
    public class VGroupUser : Entity, ITimestamp
    {
        public Guid VGroupId { get; set; }

        public String UserId { get; set; }

        [ForeignKey("VGroupId")]
        public virtual VGroup VGroup { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}