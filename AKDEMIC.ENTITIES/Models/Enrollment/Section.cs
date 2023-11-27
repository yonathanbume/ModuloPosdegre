using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.Laurassia;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Section : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid CourseTermId { get; set; }
        public Guid? GroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        public bool IsDirectedCourse { get; set; }
        public byte Vacancies { get; set; } = 0;
        //public bool GradedEvaluations { get; set; }

        [NotMapped]
        public int StudentsCount { get; set; }

        [NotMapped]
        public Guid? StudentSectionId { get; set; }

        public int? MoodleId { get; set; }

        [NotMapped]
        public IEnumerable<string> TeacherNames { get; set; }

        public CourseTerm CourseTerm { get; set; }
        public Group Group { get; set; }
        public ICollection<Content> Contents { get; set; }
        public ICollection<Class> Classes { get; set; }
        public ICollection<ClassSchedule> ClassSchedules { get; set; }
        public ICollection<StudentSection> StudentSections { get; set; }
        public ICollection<TeacherSection> TeacherSections { get; set; }
        public ICollection<TermInform> TermInforms { get; set; }
        public ICollection<TmpEnrollment> TmpEnrollments { get; set; }
        public ICollection<GradeRegistration> GradeRegistrations { get; set; }
        public ICollection<EvaluationReport> EvaluationReports { get; set; }
        public ICollection<WeeklyAttendanceReport> WeeklyAttendanceReports { get; set; }
        public ICollection<AcademicHistory> AcademicHistories { get; set; }
        public ICollection<VExam> VExams { get; set; }
        public ICollection<VForum> VForums { get; set; }
        public ICollection<Homework> Homeworks { get; set; }
        public ICollection<Reading> Readings { get; set; }
        public ICollection<VirtualClass> VirtualClasses { get; set; }
        public ICollection<PerformanceEvaluationUser> PerformanceEvaluationUsers { get; set; }


        public ICollection<SubstituteExam> SubstituteExams { get; set; }
    }
}
