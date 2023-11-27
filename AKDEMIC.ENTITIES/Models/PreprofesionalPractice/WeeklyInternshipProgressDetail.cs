using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.PreprofesionalPractice
{
    public class WeeklyInternshipProgressDetail
    {
        public Guid Id { get; set; }
        
        public Guid WeeklyInternshipProgressId { get; set; }
        public WeeklyInternshipProgress WeeklyInternshipProgress { get; set; }

        public Guid InternshipAspectId { get; set; }
        public InternshipAspect InternshipAspect { get; set; }

        public string Description { get; set; }
    }
}
