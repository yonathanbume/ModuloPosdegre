using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class StudentPaymentStatus
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Career { get; set; }
        public string Faculty { get; set; }
        public string Curriculum { get; set; }
        public decimal Credits { get; set; }
        public string CurriculumCode { get; set; }
        public bool Paid { get; set; }
        public int AcademicYear { get; set; }
    }
}
