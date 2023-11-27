using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class UnitActivity
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Week { get; set; }

        public int Order { get; set; }

        public Guid CourseUnitId { get; set; }

        public CourseUnit CourseUnit { get; set; }

        public ICollection<Class> Classes { get; set; }

        [NotMapped]
        public string AccomplishedTime { get; set; }
    }
}
