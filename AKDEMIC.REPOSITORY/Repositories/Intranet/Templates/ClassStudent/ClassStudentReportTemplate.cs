using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.ClassStudent
{
    public class ClassStudentReportTemplate
    {
        public Guid StudentId { get; set; }
        public Guid? SectionGroupId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int Absences { get; set; }
        public int Assisted { get; set; }
        public int MaxAbsences { get; set; }
        public int Dictated { get; set; }
        public int TotalClasses { get; set; }
        public decimal AbsencesPercentage { get; set; }
        public List<ClassStudentDetailReportTemplate> ClassStudentDetail { get; set; }
    }

    public class ClassStudentDetailReportTemplate
    {
        public Guid ClassId { get; set; }
        public bool IsAbsent { get; set; }
    }
}
