using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class FutureGradutedStudentTemplate
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Career { get; set; }
        public decimal ApprovedCredits { get; set; }
        public decimal EnrolledCredits { get; set; }
    }
}
