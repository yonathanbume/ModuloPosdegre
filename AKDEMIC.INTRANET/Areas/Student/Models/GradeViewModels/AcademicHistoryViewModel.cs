using System;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.INTRANET.Areas.Student.Models.GradeViewModels
{
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
        public bool EvaluationByUnits { get; set; }
        public int CurriculumAcademicYear { get; set; }
        public CourseSyllabusViewModel Syllabus { get; set; }
    }

    public class CourseSyllabusViewModel
    {
        public bool Enabled { get; set; }
        public Guid SyllabusTeacherId { get; set; }
    }

    public class StudentSectionViewModel
    {
        public Guid Id { get; set; }
        public SectionViewModel Section { get; set; }

        public int Try { get; set; }

        public string Observations { get; set; }

        public int Status { get; set; }

        public decimal MinGradeTerm { get; set; }

        public string Formula => Section.CourseUnits != null ?  Section.CourseUnits.All(y => y.AcademicProgressPercentage == 0) ? $"({string.Join("+", Section.CourseUnits.Select(x => $"U{x.Number}").ToList())})/{Section.CourseUnits.Count()}" : $"{string.Join("+", Section.CourseUnits.Select(x => $"U{x.Number} * {x.AcademicProgressPercentage}%").ToList())}" :
            string.Join(" + ", Section.Evaluations.Select(y=>$"({y.FormattedName}*{y.Percentage}%)").ToList());

        public bool Approved => FinalGrade >= MinGradeTerm;

        public decimal PercentageProgress { get; set; }

        public int FinalGrade { get; set; }
        public decimal? SubstituteExamFinalGrade { get; set; }
    }

    public class SectionViewModel
    {
        public CourseTermViewModel CourseTerm { get; set; }
        public IEnumerable<CourseUnitViewModel> CourseUnits { get; set; }
        public IEnumerable<EvaluationViewModel> Evaluations { get; set; }
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

    public class CourseUnitViewModel
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public decimal GradeByUnit { get; set; }
        public int AcademicProgressPercentage { get; set; }
        public List<EvaluationViewModel> Evaluations { get; set; }
    }

    public class EvaluationViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FormattedName => Name.Length > 20 ? $"{Name.Substring(0, 15)}..." : Name;
        public string Description { get; set; }
        public int Percentage { get; set; }
        public GradeViewModel Grade { get; set; }
    }

    public class GradeViewModel
    {
        public bool HasGrade { get; set; }
        public bool Attended { get; set; }
        public decimal Value { get; set; }
        public bool Approved { get; set; }
    }


}
