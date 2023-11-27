using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Tutoring.Templates
{
    public class TutorWorkingPlanTemplate
    {
        public Guid Id { get; set; }
        public string TutorId { get; set; }
        public string TutorFullName { get; set; }
        public Guid TermId { get; set; }
        public string TermName { get; set; }
        public string WorkingPlanPath { get; set; }
    }
}
