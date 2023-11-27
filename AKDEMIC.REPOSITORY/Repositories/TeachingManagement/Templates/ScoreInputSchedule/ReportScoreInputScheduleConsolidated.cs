using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.ScoreInputSchedule
{
    public class ReportScoreInputScheduleConsolidated
    {
        public string Section { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public bool HasEvaluations { get; set; }
        public int CourseUnitsQuantity { get; set; }
        public bool Complete { get; set; }
        public string Teacher { get; set; }
        public int Evaluations { get; set; }
    }
}
