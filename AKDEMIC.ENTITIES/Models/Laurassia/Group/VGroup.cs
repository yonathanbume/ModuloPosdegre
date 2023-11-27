using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Laurassia
{
    [Table("VGroup")]
    public class VGroup : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public String Description { get; set; }

        [Required]
        public String State { get; set; }

        [Required]
        public Guid SectionId { get; set; }

        [ForeignKey("SectionId")]
        public virtual Section Section { get; set; }

        public ICollection<VGroupUser> VGroupUser { get; set; }

    }
}
