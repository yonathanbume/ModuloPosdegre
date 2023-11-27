using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluationUser
{
    public class PerformanceEvaluationUserReportTemplate
    {
        public bool VigesimalScale { get; set; }

        public string PerformanceEvaluation { get; set; }
        public string AcademicDepartment { get; set; }
        public int TeachersSurveyed { get; set; }
        public decimal Average { get; set; }
        public string Image { get; set; }
        public string Term { get; set; }
        public byte Target { get; set; }
        public List<PerformanceEvaluationUserReportDetailTemplate> Users { get; set; }
    }

    public class PerformanceEvaluationUserReportDetailTemplate
    {
        public string AcademicDepartment { get; set; }

        public int Number { get; set; }
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string UserFullName { get; set; }
        public PerformanceEvaluationStudentTemplate StudentSameCareer { get ; set; }
        public PerformanceEvaluationStudentTemplate StudentOtherCareer { get; set; }

        public decimal VigesimalStudentFinalGrade { get; set; }
        public decimal VigesimalDeanFinalGrade { get; set; }
        public decimal VigesimalAcademicDepartmentDirectorFinalGrade { get; set; }
        public decimal VigesimalCareerDirectorFinalGrade { get; set; }
        public decimal VigesimalResearchCoordinatorFinalGrade { get; set; }
        public decimal VigesimalSocialResponsabilityCoordinatorFinalGrade { get; set; }
        public decimal VigesimalTutoringCoordinatorFinalGrade { get; set; }
        public decimal VigesimalFinalGradeAuthorities { get ; set; }
        public decimal VigesimalFinalGrade { get; set; }

        public decimal TotalStudentFinalGrade { get; set; }
        public decimal TotalDeanFinalGrade { get; set; }
        public decimal TotalAcademicDepartmentDirectorFinalGrade { get; set; }
        public decimal TotalCareerDirectorFinalGrade { get; set; }
        public decimal TotalResearchCoordinatorFinalGrade { get; set; }
        public decimal TotalSocialResponsabilityCoordinatorFinalGrade { get; set; }
        public decimal TotalTutoringCoordinatorFinalGrade { get; set; }
        public decimal TotalFinalGradeAuthorities { get; set; }
        public decimal TotalFinalGrade { get; set; }


    }

    public class PerformanceEvaluationStudentTemplate
    {
        public int Total { get; set; }
        public int Answered { get; set; }
        public int Remaining { get; set; }
        public decimal VigesimalFinalGrade { get; set; }
        public decimal TotalFinalGrade { get; set; }
    }
}
