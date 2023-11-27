using AKDEMIC.INTRANET.Helpers;
using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.ViewModels.AcademicSummaryViewModels
{
    public class RecordReportViewModel
    {
        //GENERAL
        public string HeaderText { get; set; }
        public string FacultyName { get; set; }
        public string CareerName { get; set; }
        public string StudentFullName { get; set; }
        public string StudentCode { get; set; }
        public string StudentDni { get; set; }
        public string CurrentTerm { get; set; }
        public string ImagePath { get; set; }
        public string StudentCurriculum { get; set; }
        public List<StudentTermReportViewModel> Terms { get; set; }

        //UNJBG
        public string Regime { get; set; }
        public string CurriculumCode { get; set; }
        public string Modality { get; set; }
        public List<StudentAcademicYearViewModel>  AcademicYears { get; set; }
    }

    public class StudentTermReportViewModel
    {
        public string TermName { get; set; }

        public string CareerName { get; set; }

        public List<StudentSectionsViewModel> StudentSections { get; set; }

        public SummaryViewModel AcademicSummary { get; set; }
    }

    public class StudentSectionsViewModel
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public int AcademicYear { get; set; }
        public decimal Credits { get; set; }
        public string FinalGrade { get; set; }
        public string FinalGradeText => (FinalGrade == "-" || FinalGrade == "Retirado") ? "-" : ConvertHelpers.NumberToText(FinalGrade);
        public bool NotDisapproved { get; set; }
        public int Status { get; set; }
        public int Try { get; set; }
        public Guid CourseId { get; set; }
        public Guid CurriculumId { get; set; }
        public byte? Type { get; set; }
        public string Date { get; set; }
    }


    //UNJBG
    public class StudentAcademicYearViewModel
    {
        public string AcademicYearName { get; set; }
        public SummaryViewModel AcademicSummary { get; set; }
        public List<StudentCourseViewModel> Courses { get; set; }
    }

    public class StudentCourseViewModel
    {
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public decimal Credits { get; set; }
        public string ActType { get; set; }
        public List<AcademicHistoryViewModel> AcademicHistories { get; set; }
    }

    public class AcademicHistoryViewModel
    {
        public string TermName { get; set; }
        public int FinalGrade { get; set; }
        public DateTime? FinalGradeDateTime { get; set; }
        public bool Withdraw { get; set; }
        public bool IsValidated { get; set; }
        public bool Approbed { get; set; }
    }

}
