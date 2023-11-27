using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CourseType : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
