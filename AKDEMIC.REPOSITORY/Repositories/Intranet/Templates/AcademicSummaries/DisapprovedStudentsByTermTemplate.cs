using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.AcademicSummaries
{
    public class DisapprovedStudentsByTermTemplate
    {
        public string Term { get; set; }

        public int DisapprovedStudents { get; set; }

        public int TotalStudents { get; set; }
    }
}
