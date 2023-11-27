using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.TeachingManagement;

namespace AKDEMIC.ENTITIES.Models
{
    public class Contract : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public DateTime Begin { get; set; }

        [Required]
        public string Commentario { get; set; }
        public DateTime End { get; set; }

        [Required]
        public string Resolution { get; set; }
    }
}
