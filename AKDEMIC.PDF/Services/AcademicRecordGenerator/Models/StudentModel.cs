using System.Collections.Generic;

namespace AKDEMIC.PDF.Services.AcademicRecordGenerator.Models
{
    public class StudentModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Career { get; set; }
        public string Faculty { get; set; }
        public string Campus { get; set; }

        //
        public int ApprovedCourses { get; set; }
        public decimal ApprovedCredits { get; set; }
        public decimal LastWeightedAverageGrade { get; set; }
        public decimal AverageApprovedCredits { get; set; }

        public List<string> WithdrawalTerms { get; set; }
        public List<string> ReservedTerms { get; set; }
        public List<string> SuspendedTerms { get; set; }
        public List<string> AbandonedTerms { get; set; }

    }
}
