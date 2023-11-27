using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport
{
    public class ExtraordinaryEvaluationReportTemplate
    {
        public ExtraordinaryEvaluationReportCourseTemplate Course { get; set; }
        public ExtraordinaryEvaluationReportTeacherPresidentTemplate TeacherPresident { get; set; }
        public EvaluationReportTermTemplate Term { get; set; }

        public List<ExtraordinaryEvaluationStudentsTemplate> ExtraordinaryEvaluationStudents { get; set; }
        public string Img { get; set; }
        public string UserLoggedIn { get; set; }
        public string Header { get; set; }
        public string SubHeader { get; set; }
    }

    public class ExtraordinaryEvaluationReportCourseTemplate
    {
        public string Code { get; set; }
        public bool IsElective { get; set; }
        public string Name { get; set; }
        public string Curriculum { get; set; }
        public string Credits { get; set; }
        public int AcademicYear { get; set; }
        public string CampusName { get; set; }
        public string CareerName { get; set; }
    }

    public class ExtraordinaryEvaluationReportTeacherPresidentTemplate
    {
        public string FullName { get; set; }
        public string AcademicDepartment { get; set; }
    }

    public class ExtraordinaryEvaluationStudentsTemplate
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public decimal Grade { get; set; }
        public string GradeText { get; set; }
        public string Observations { get; set; }
    }
}
