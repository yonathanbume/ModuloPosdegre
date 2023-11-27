using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Area : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public bool IsSpecialty { get; set; } = false;

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
