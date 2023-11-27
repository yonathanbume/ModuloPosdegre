using AKDEMIC.INTRANET.Areas.Admin.Models.CertificateViewModels;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class StudyRecordViewModel
    {
        public int Sex { get; set; }
        public string University { get; set; }

        public string Faculty { get; set; }

        public string Student { get; set; }

        public string Code { get; set; }

        public string AcademicYear { get; set; }

        public string Term { get; set; }

        public string Location { get; set; }

        public string Date { get; set; }

        public string PathLogo { get; set; }
        public string PageCode { get;  set; }
        public string ImageQR { get; set; }
        public UniversityInformation UniversityInformation { get; set; }

    }

}
