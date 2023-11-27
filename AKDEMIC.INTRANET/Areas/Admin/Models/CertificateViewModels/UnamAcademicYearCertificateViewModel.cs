using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels
{
    public class UnamAcademicYearCertificateViewModel
    {
        public int AcademicYearNumber { get; set; }
        public IList<UnamCourseCertificateViewModel> Courses { get; set; }
    }
}
