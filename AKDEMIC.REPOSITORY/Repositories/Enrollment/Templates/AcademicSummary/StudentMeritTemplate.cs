using System;
using System.Collections.Generic;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.AcademicSummary
{
    public class StudentMeritTemplate
    {
        public Guid StudentId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Grade { get; set; }

        public decimal ApprovedCredits { get; set; }

        public decimal DisapprovedCredits { get; set; }

        public int ApprovedCreditsIsGreater { get; set; }

        public int EnrolledCreditsIsGreater { get; set; }

        public int ApprovedAllCredits { get; set; }

        public int EnrolledCreditsIsGreaterThanTwelve { get; set; }

        public int PriorityLevel { get; set; }

        public int MeritOrder { get; set; }

        public int TotalOrder { get; set; }

        public int MeritType { get; set; }

        public string OrderPosition { get; set; }

        public int MaxTry { get; set; }
    }
    public class CurriculumTemplate
    {
        public Guid Id { get; set; }

        public List<AcademicYearTemplate> AcademicYears { get; set; }
    }

    public class AcademicYearTemplate
    {
        public int Number { get; set; }

        public decimal Credits { get; set; }

        public decimal RequiredCredits { get; set; }

        public decimal PrevCredits { get; set; }
    }

}
