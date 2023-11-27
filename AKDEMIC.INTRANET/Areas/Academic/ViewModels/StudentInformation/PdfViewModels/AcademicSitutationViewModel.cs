using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation.PdfViewModels
{
    public class AcademicSitutationViewModel
    {
        public string StudentUsername { get; set; }
        public string Student { get; set; }
        public string Career { get; set; }
        public string AcademicProgram { get; set; }
        public string ImagePathLogo { get; set; }

        public decimal WeightedFinalGrade { get; set; }
        public decimal CumulativeWeightedFinalGrade { get; set; }
        public int Order { get; set; }
        public decimal RequiredApprovedCredits { get; set; }
        public decimal RequiredCredits { get; set; }
        public decimal ElectiveApprovedCredits { get; set; }
        public decimal ElectiveCredits { get; set; }
        public List<AcademicSitutationDetailViewmodel> Details { get; set; }
    }

    public class AcademicSitutationDetailViewmodel
    {
        public int AcademicYear { get; set; }
        public string CourseCode { get; set; }
        public string Course { get; set; }
        public string Credits { get; set; }
        public string FinalGrade { get; set; }
        public string Semester { get; set; }
        public string Status { get; set; }
    }
}
