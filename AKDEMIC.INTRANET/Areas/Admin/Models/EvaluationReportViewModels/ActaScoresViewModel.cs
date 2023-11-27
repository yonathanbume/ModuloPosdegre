using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.EvaluationReportViewModels
{
    public class ActaScoresViewModel
    {
        public string Img { get; set; }
        public BasicInformation BasicInformation { get; set; }
        public List<Row> Rows { get; set; }
        public byte Type { get; set; }
        public decimal Approbed { get; set; }
        public string Sender { get; set; }
        public string CareerDirector { get; set; }
        //public Total Total { get; set; }
        public string FinalQR { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public DateTime? LastGradeRegistration { get; set; }
        public List<Evaluation> Evaluations { get; set; }
    }
    public class BasicInformation
    {
        public string Teacher { get; set; }
        public string Course { get; set; }
        public string Faculty { get; set; }
        public string AcademicProgram { get; set; }
        public string Career { get; set; }
        public string Credits { get; set; }
        public string Sede { get; set; }
        public string Term { get; set; }
        public string Cycle { get; set; }
        public string Section { get; set; }
        public int TotalUnits { get; set; }
        public byte? TheoreticalHours { get; set; }
        public byte? PracticalHours { get; set; }
        public int? EffectiveHours { get; set; }
        public int CourseUnits { get; set; }
        public int Evaluations { get; set; }
        public List<Evaluation> EvaluationsList { get; set; } = new List<Evaluation>();
        public bool EvaluationByUnits { get; set; }
        public List<CourseUnit> CourseUnitsList { get; set; }
        public string ReceptionDate { get; set; }
        public string User { get; set; }
        public string Signature { get; set; }
        public bool? IsSummer { get; set; }
        public decimal MinGrade { get; set; }
        public string Curriculum { get; set; }
        public List<string> Committee { get; set; }
        public string Resolution { get; set; }
    }
    public class Row
    {
        public int Order { get; set; }
        public int StudentStatus { get; set; }
        public string Code { get; set; }
        public string Surnames { get; set; }
        public string Names { get; set; }
        public int?[] PartialAverages { get; set; }
        public decimal[] PartialEvaluationAverages { get; set; }
        public int Try { get; set; }
        public int? FinalEvaluation { get; set; }
        public int? FinalEvaluationNumber { get; set; }
        public string FinalEvaluationText { get; set; }
        public bool HasSusti { get; set; }
        public string FinalEvaluationApprobed { get; set; }
        public List<Grade>Grades { get; set; }
        public Guid StudentSectionId{ get; set; }
    }
}
