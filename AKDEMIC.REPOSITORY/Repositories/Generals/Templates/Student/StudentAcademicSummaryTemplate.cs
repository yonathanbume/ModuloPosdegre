using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class StudentAcademicSummaryTemplate
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string FullName { get; set; }

        public string Career { get; set; }

        public string Faculty { get; set; }

        public int AcademicYear { get; set; }

        public decimal TotalCredits { get; set; }

        public decimal ApprovedCredits { get; set; }
    }
}
