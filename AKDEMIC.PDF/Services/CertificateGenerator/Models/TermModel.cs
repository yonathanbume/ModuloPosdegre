using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.PDF.Services.CertificateGenerator.Models
{
    public class TermModel
    {
        public DateTime EnrollmentStartDate { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }
        public int? Number { get; set; }
        public int EnrolledCourses { get; set; }
        public decimal EnrolledCredits { get; set; }
        public int StudentStatus { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MeritOrder { get; set; }
        public int? TotalMeritOrder { get; set; }
        public decimal? WeightedAverageGrade { get; set; }
        public int? AcademicYear { get; set; }
    }
}
