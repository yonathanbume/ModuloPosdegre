using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels
{
    public class UnamCertificateViewModel
    {
        public IList<UnamAcademicYearCertificateViewModel> AcademicYears { get; set; }
        public UnamHeaderCertificateViewModel Header { get; set; }
        public string ImagePathLogo { get; set; }
        public DateTime Today { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal CurrentWeightedGrade { get; set; }
        public decimal MinPassingGrade { get; set; }
        public decimal MaxPassingGrade { get; set; }
    }
}
