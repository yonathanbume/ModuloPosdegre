using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicYearCourse
{
    public class AcademicSummaryDetailTemplate
    {
        public string Course { get; set; }
        public string Credits { get; set; }
        public int Try { get; set; }
        public string FinalGrade { get; set; }
        public int Status { get; set; }
        public int AcademicYear { get; set; }
    }
}
