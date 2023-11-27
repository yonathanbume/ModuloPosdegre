using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicHistory
{
    public class AcademicHistoryCourseTemplate
    {
        public int AcademicYear { get; set; }
        public string Year { get; set; }
        public string Code { get; set; }
        public string Course { get; set; }
        public decimal Credits { get; set; }
        public int Grade { get; set; }
        public string Term { get; set; }
        public string Status { get; set; }
        public bool Validated { get; set; }
        public bool Approved { get; set; }
        public string Observations { get; set; }
        public string AcademicObservations { get; set; }
        public byte Type { get; set; }
        public int Try { get; set; }
        public bool Withdraw { get; set; }
        public string EvaluationReportCode { get; set; }
        public DateTime? EvaluationReportDate { get; set; }
    }
}
