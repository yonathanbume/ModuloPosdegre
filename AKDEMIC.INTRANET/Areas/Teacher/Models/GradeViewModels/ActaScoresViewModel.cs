using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.GradeViewModels
{
    public class ActaScoresViewModel
    {
        public string HeaderText { get; set; }
        public string SubHeaderText { get; set; }
        public string Img { get; set; }
        public BasicInformation BasicInformation { get; set; }
        public List<Row> Rows { get; set; }
        public decimal Approbed { get; set; }
        public Total Total { get; set; }
    }
    public class BasicInformation
    {
        public string Teacher { get; set; }
        public string TeacherAcademicDepartment { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Career { get; set; }
        public string Credits { get; set; }
        public string Sede { get; set; }
        public string Curriculum { get; set; }
        public string Term { get; set; }
        public string Type { get; set; }
        public int Cycle { get; set; }
        public string Section { get; set; }
        public string User { get;  set; }
        public decimal MinGrade { get;  set; }
        public string Faculty { get;  set; }
        public string AcademicProgram { get;  set; }
        public List<CourseUnit> CourseUnitsList { get;  set; }
        public int CourseUnits { get;  set; }
        public decimal CompletionPercentage { get; set; }

    }
    public class Row
    {
        public int StudentStatus { get; set; }
        public int Order { get; set; }
        public string Code { get; set; }
        public string Surnames { get; set; }
        public string Names { get; set; }
        public string AssistancePercent { get; set; }
        public string Email { get; set; }
        public string RegularEvaluation { get; set; }
        public int RegularEvaluationNumber { get; set; }
        public string RegularEvaluationText { get; set; }
        public string SustEvaluation { get; set; }
        public string SustEvaluationText { get; set; }
        public string FinalEvaluation { get; set; }
        public int FinalEvaluationNumber { get; set; }
        public string FinalEvaluationText { get; set; }
        public string FinalEvaluationApprobed { get; set; }
        public bool Withdrawn { get; set; }
        public int?[] PartialAverages { get;  set; }
        public bool HasSusti { get;  set; }
        public bool HasGrades { get; set; }
        public Guid StudentSectionId { get;  set; }
        public string PhoneNumber { get; set; }
    }
    public class Total
    {
        public int Enrollment { get; set; }
        public int Approved { get; set; }
        public int NotApproved { get; set; }
        public int Sust { get; set; }
    }
}
