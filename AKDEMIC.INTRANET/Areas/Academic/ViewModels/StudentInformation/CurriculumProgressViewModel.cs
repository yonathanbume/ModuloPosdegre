using System;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class CurriculumProgressViewModel
    {
        public Guid Id { get; set; }
        public decimal TotalAppovedCredits { get; set; }
        public int TotalApprovedCourses { get; set; }
        public bool HasFile { get; set; }
    }
}
