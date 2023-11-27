using System;
using AKDEMIC.ENTITIES.Models.Generals;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    public class VForumChild : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? VForumChildParentId { get; set; }
        public VForumChild VForumChildParent { get; set; }
        public Guid VForumId { get; set; }
        public string StudentId { get; set; }
        public DateTime DateRegister { get; set; }
        public decimal Score { get; set; }
        public bool Rated { get; set; }
        public string Description { get; set; }
        public virtual ApplicationUser Student { get; set; }
        public virtual VForum VForum { get; set; }
        public DateTime QualificationDate { get; set; }
        public string QualificationTeacherId { get; set; }
        public ICollection<VForumChild> VForumChilds { get; set; }
        public ICollection<VForumChildFile> VForumChildFiles { get; set; }
    }
}
