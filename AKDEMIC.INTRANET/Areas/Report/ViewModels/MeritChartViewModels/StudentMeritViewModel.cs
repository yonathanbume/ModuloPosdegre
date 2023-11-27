using System;

namespace AKDEMIC.INTRANET.Areas.Report.ViewModels.MeritChartViewModels
{
    public class StudentMeritViewModel
    {
        public Guid StudentId { get; set; }

        public string Code { get; set; }

        public string CurriculumCode { get; set; }

        public string Name { get; set; }

        public string AcademicProgram { get; set; }

        public decimal Grade { get; set; }

        public decimal AcademicYearCredits { get; set; }

        public decimal ApprovedCredits { get; set; }

        public decimal DisapprovedCredits { get; set; }

        public int MeritOrder { get; set; }

        public int TotalOrder { get; set; }

        public int MeritType { get; set; }

        public int MaxTry { get; set; }
    }
}
