using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.CertificateGenerator.Models
{
    public class StudentModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public int AcademicYear { get; set; }
        public string Document { get; set; }
        public int Sex { get; set; }
        public decimal ApprovedCredits { get; set; }
        public TermModel FirstEnrollment { get; set; }
        public TermModel RecordTerm { get; set; }
        public TermModel AdmissionTerm { get; set; }
        public TermModel GraduationTerm { get; set; }
        public PostulantModel Postulant { get; set; }
    }
}
