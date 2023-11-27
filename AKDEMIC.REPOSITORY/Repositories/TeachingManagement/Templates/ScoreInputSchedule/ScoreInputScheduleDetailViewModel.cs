using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.ScoreInputSchedule
{
    public class ScoreInputScheduleDetailViewModel
    {
        public Guid SectionId { get; set; }
        public string Teachers { get; set; }
        public string Career { get; set; }
        public string Course { get; set; }
        public string Section { get; set; }
        public string Cycle { get; set; }
        public string Component { get; set; }
        public byte UnitNumber { get; set; }
        public bool AllEvaluationWithGrades { get; set; }
        public string LastGradeRegistration { get; set; }
        public DateTime LastGradeRegistationDateTime { get; set; }
        public bool WasLate { get; set; }
        public string Status { get; set; }
        public bool TimeIsUp { get; set; }

        public string Evaluation { get; set; }
        public int? EvaluationWeek { get; set; }
        public bool RegisterOnClass { get; set; }
        public bool HasGradeRegistration { get; set; }
        ///
        public int TotalEvaluations { get; set; }
        public int EvaluationsWithGrades { get; set; }
        public string AcademicDepartments { get; set; }
        public string TeacherConditions { get; set; }
        public string TeacherDedications { get; set; }
    }
}
