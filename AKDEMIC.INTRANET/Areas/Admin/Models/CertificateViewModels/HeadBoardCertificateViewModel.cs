using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels
{
    public class HeadBoardCertificateViewModel
    {
        public Guid IdStudent { get; set; }
        public string FacultyName { get; set; }
        public string CareerName { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string AdmissionYear { get; set; }
        public string GraduationYear { get; set; }
        public Guid CurriculumId { get; set; }
        public string Curriculum { get; set; }
        public string Dni { get; set; }
        public string AcademicProgram { get; set; }
        public int StudentSex { get; set; }
        public string BachelorName { get; set; }

        public decimal TotalApprovedMandatoryCredits { get; set; }
        public decimal TotalApprovedCredits { get; set; }
        public decimal TotalApprovedElectiveCredits { get; set; }
        public decimal WeightedAverageCumulative { get; set; }
    }
}
