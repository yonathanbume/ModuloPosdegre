using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.GradeViewModels
{
    public class DetailViewModel
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Career { get; set; }

        public string Term { get; set; }

        public Guid ActiveTerm { get; set; }

        public List<SelectListItem> Terms { get; set; }
    }

    public class TermViewModel
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public decimal MinimumValue { get; set; }
    }

    public class AcademicHistoryViewModel
    {
        public TermViewModel Term { get; set; }

        public int SectionCount => EnrolledCourses.Count();

        public decimal CreditSum => EnrolledCourses.Sum(x => x.StudentSection.Section.CourseTerm.Credits);

        public int AcademicYear => EnrolledCourses.Min(x => x.CurriculumAcademicYear);

        public IEnumerable<EnrolledCourseViewModel> EnrolledCourses { get; set; }
    }

    public class EnrolledCourseViewModel
    {
        public StudentSectionViewModel StudentSection { get; set; }

        public int CurriculumAcademicYear { get; set; }
    }

    public class StudentSectionViewModel
    {
        public SectionViewModel Section { get; set; }

        public int Try { get; set; }

        public string Observations { get; set; }

        public int Status { get; set; }

        public decimal MinGradeTerm { get; set; }

        public string Formula => string.Join(" + ", Section.Grades.Select(x => x.Evaluation.Percentage.ToString() + "% (" + x.Evaluation.Name + ")"));

        public bool Approved => CurrentFinalGrade >= MinGradeTerm;

        public decimal PercentageProgress => Section.Grades.Where(x => !x.IsDefaultValue).Sum(x => x.Evaluation.Percentage);

        public decimal CurrentFinalGrade => PercentageProgress == 0 ? 0.00M : Section.Grades.Where(x => !x.IsDefaultValue).Sum(x => x.Value * x.Evaluation.Percentage / 100) / (PercentageProgress / 100);

        public int FinalGrade { get; set; }
    }

    public class SectionViewModel
    {
        public CourseTermViewModel CourseTerm { get; set; }

        public IEnumerable<GradeViewModel> Grades { get; set; }

        public string Code { get; set; }
    }

    public class CourseTermViewModel
    {
        public CourseViewModel Course { get; set; }

        public string TemaryUrl { get; set; }

        public decimal Credits { get; set; }
    }

    public class CourseViewModel
    {
        public string FullName { get; set; }
    }

    public class EvaluationViewModel
    {
        public string Name { get; set; }

        public int Percentage { get; set; }
    }

    public class GradeViewModel
    {
        public EvaluationViewModel Evaluation { get; set; }

        public decimal Value { get; set; }

        public bool Attended { get; set; }

        public bool Approved { get; set; }

        public bool IsDefaultValue { get; set; }
    }
}
