using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels
{
    public class CertificateCompleteViewModel
    {
        public List<CertificateViewModel> Certificate { get; set; }
        public HeadBoardCertificateViewModel HeaderBoard { get; set; }
        public string ImagePathLogo { get; set; }
        public string Today { get; set; }  
        public decimal YearOfStudies { get; set; }
        public decimal CumulativeWeightedAverage { get; set; }
        public string ImgQR { get; set; }
        public string User { get; set; }
        public UniversityInformation University { get; set; }
    }
}
