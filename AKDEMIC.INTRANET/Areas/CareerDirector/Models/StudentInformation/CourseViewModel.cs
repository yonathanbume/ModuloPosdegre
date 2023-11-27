using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Models.StudentInformation
{
    public class CourseViewModel
    {
        public string Course { get; set; }

        public int Try { get; set; } = 1;

        public string Observations { get; set; }

        public decimal FinalGrade { get; set; }
        
        public string Formula { get; set; }

        public int Status { get; set; }

        public string Section { get; set; }

        public decimal Credits { get; set; }

        public decimal PercentageProgress { get; set; }

        public decimal CurrentFinalGrade => PercentageProgress == 0 ? 0.00M : Evaluations.Where(x => x.Taked).Sum(x => x.Grade * x.Percentage / 100) / (PercentageProgress / 100);

        public bool Approved { get; set; }

        public List<CourseEvaluationViewModel> Evaluations { get; set; }
    }
}
