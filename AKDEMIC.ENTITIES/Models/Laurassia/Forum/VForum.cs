using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    [Table("VForum")]
    public class VForum : Entity, ITimestamp
    {
        [Key]
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public bool QualificationMode { get; set; }
        public string TeacherId { get; set; }
        public virtual ApplicationUser Teacher { get; set; }
        public Guid ContentId { get; set; }
        public virtual Content Content { get; set; }
        public bool Show { get; set; } = true;
        public byte? MaxComments { get; set; }
        public virtual ICollection<VForumChild> VForumChild { get; set; }
        public virtual ICollection<VForumFile> VForumFiles { get; set; }
    }
}
