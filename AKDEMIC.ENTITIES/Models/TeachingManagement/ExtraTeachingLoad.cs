using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class ExtraTeachingLoad
    {
        [Key]
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        [Key]
        public Guid TermId { get; set; }
        public Term Term { get; set; }

        public decimal EvaluationHours { get; set; }
        public decimal OtherAcademicActivities { get; set; }
    }
}
