using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluationUser
{
    public class ComplianceDetailedReport
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Career { get; set; }
        public string Curriculum { get; set; }
        public int Pending { get; set; }
        public int Total { get; set; }
        public int Answered { get; set; }
    }
}
