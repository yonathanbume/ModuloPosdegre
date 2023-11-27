using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluation
{
    public class CommentaryReport
    {
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string Term { get; set; }
        public string AcademicDepartment { get; set; }
        public List<CommentaryByTeacher> Details { get; set; }
    }

    public class CommentaryByTeacher
    {
        public string Teacher { get; set; }
        public string Commentary { get; set; }
        public string Career { get; set; }
        public string AcademicDepartment { get; set; }
    }
}
