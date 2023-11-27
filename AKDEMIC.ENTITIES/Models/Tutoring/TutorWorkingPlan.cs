using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Tutoring
{
    public class TutorWorkingPlan : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public string WorkingPlanPath { get; set; }
        [Required]
        public string TutorId { get; set; }
        public Tutor Tutor { get; set; }
        public Guid TermId { get; set; }
        public Term Term { get; set; }
    }
}
