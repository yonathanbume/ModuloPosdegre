using AKDEMIC.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport
{
    public class EvaluationReportInformationTemplate
    {
        public EvaluationReportBasicInformationTemplate BasicInformation { get; set; }
        public EvaluationReportCourseTemplate Course { get; set; }
        public EvaluationReportTermTemplate Term { get; set; }
        public string FinalQR { get; set; }
        public string Img { get; set; }
        public string ImgNationalEmblem { get; set; }
        public string UserLoggedIn { get; set; }
        public string UserLoggedInFullName { get; set; }
        public string DocumentTitle { get; set; }
        public string Header { get; set; }
        public string SubHeader { get; set; }
    }

    public class EvaluationReportBasicInformationTemplate
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public byte Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public DateTime? LastGradeRegistration { get; set; }
        public DateTime? DateByConfiguration { get; set; }
        public byte Status { get; set; }
        public string ResolutionNumber { get; set; }
        public List<string> Comittee { get; set; }
        public byte? ExtraordinaryEvaluationType { get; set; }
    }

    public class EvaluationReportTermTemplate
    {
        public string Name { get; set; }
        public bool IsSummer { get; set; }
        public decimal MinGrade { get; set; }
        public int Status { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class EvaluationReportCareerTemplate
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string CareerDirector { get; set; }
        public string Faculty { get; set; }
    }

    public class EvaluationReportCourseTemplate
    {
        public string Code { get; set; }
        public bool IsElective { get; set; }
        public string Name { get; set; }
        public string Curriculum { get; set; }
        public string Credits { get; set; }
        public int AcademicYear { get; set; }
        public byte TheoreticalHours { get; set; }
        public byte PracticalHours { get; set; }
        public int EffectiveHours => TheoreticalHours + PracticalHours;
        public string Formula { get; set; }
        public string CampusName { get; set; }
        public EvaluationReportCareerTemplate Career { get; set; }
        public int PartialAveragesCount { get; set; }
        public bool EvaluationByUnits { get; set; }
        public List<EvaluationReportCourseUnitTemplate> CourseUnits { get; set; }
        public List<EvaluationReportEvaluationTemplate> Evaluations { get; set; }
        public EvaluationReportSectionTemplate Section { get; set; }
    }
    
    public class EvaluationReportCourseUnitTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
    }

    public class EvaluationReportEvaluationTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? Week { get; set; }
        public int Percentage { get; set; }
        public Guid? CourseUnitId { get; set; }
        public string EvaluationType { get; set; }
    }

    public class EvaluationReportSectionTemplate
    {
        public string Code { get; set; }
        public string Teacher { get; set; }
        public string AcademicDepartment { get; set; }
        public List<EvaluationReportStudent> Students { get; set; }
    }

    public class EvaluationReportPartialAverageTemplate
    {
        //CourseUnitId || EvaluationId
        public Guid Id { get; set; }
        public Guid? GradeId { get; set; }
        public int Number { get; set; }
        public bool Approved { get; set; }
        public decimal? Average { get; set; }
    }

    public class EvaluationReportGradeTempate
    {
        public Guid Id { get; set; }
        public Guid StudentSectionId { get; set; }
        public Guid? EvaluationId { get; set; }
        public decimal Value { get; set; }
    }

    public class EvaluationReportStudent
    {
        public DateTime? FinalGradePublishedDate { get; set; }
        public Guid Id { get; set; }
        public Guid StudentSectionId { get; set; }
        public Guid? SectionGroupId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool DPI { get; set; }
        public decimal AttendancePercentage { get; set; } = 100;
        public int Try { get; set; }
        public string TryText { get; set; }
        public bool HasSusti { get; set; }
        public int? GradeBeforeSusti { get; set; }
        public string GradeBeforeSustiText => GradeBeforeSusti.HasValue && ConstantHelpers.GRADES.TEXT.ContainsKey(GradeBeforeSusti.Value) ? ConstantHelpers.GRADES.TEXT[GradeBeforeSusti.Value].ToUpper() : "";
        public bool HasGradeRecovery { get; set; }
        public decimal? GradeRecoveryValue { get; set; }
        public decimal? GradeBeforeGradeRecovery { get; set; }
        public Guid? GradeIdUpdatedByGradeRecovery { get; set; }
        public int Status { get; set; }
        public int FinalGrade { get; set; }
        public bool Approved { get; set; }
        public bool HasAllGradesPublished { get; set; }
        public string FinalGradeText => ConstantHelpers.GRADES.TEXT.ContainsKey(FinalGrade) ? ConstantHelpers.GRADES.TEXT[FinalGrade].ToUpper() : "";
        public string StatusText { get; set; }
        public List<EvaluationReportPartialAverageTemplate> Averages { get; set; }
        public List<EvaluationReportGradeTempate> Grades { get; set; }
    }
}
