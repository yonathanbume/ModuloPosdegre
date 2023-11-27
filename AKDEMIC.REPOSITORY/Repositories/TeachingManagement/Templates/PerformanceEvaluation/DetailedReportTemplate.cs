using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluation
{
    public class DetailedReportTemplate
    {
        public string ToTeacherId { get; set; }
        public Guid SectionId { get; set; }
        //
        public byte AcademicYear { get; set; }
        public string Career { get; set; }
        public string Curriculum { get; set; }
        public string Section { get; set; }
        public string CodCourse { get; set; }
        public string Course { get; set; }
        public string Teacher { get; set; }
        public string AcademicDepartment { get; set; }
        public int Value { get; set; }
        public int Enrollment { get; set; }
        public int Surveys { get; set; }
    }
}
