using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class StudentGradeReportTemplate
    {
        public List<StudentGradeTableTemplate> Datatable { get; set; }
        public List<int> categories { get; set; }
        public List<int> data { get; set; }
        public string average { get; set; }
        public string standarDeviation { get; set; }
        public string median { get; set; }
        public int overmedian { get; set; }
        public int undermedian { get; set; }
        public int totalstudents { get; set; }
    }
    public class StudentGradeTableTemplate
    {
        public string Name { get;  set; }
        public int Grade{ get;  set; }
        public string SectionCode { get;  set; }
        public string Course { get;  set; }
    }
}
