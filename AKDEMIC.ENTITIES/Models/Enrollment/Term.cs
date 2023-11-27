using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class Term : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public float AbsencePercentage { get; set; }
        public bool ApplyAdditionalConcepts { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ClassEndDate { get; set; }
        public DateTime ClassStartDate { get; set; }
        public DateTime ComplementaryEnrollmentStartDate { get; set; }
        public DateTime ComplementaryEnrollmentEndDate { get; set; }
        public DateTime EnrollmentEndDate { get; set; }
        public DateTime EnrollmentStartDate { get; set; }
        public DateTime PreEnrollmentEndDate { get; set; }
        public DateTime PreEnrollmentStartDate { get; set; }
        public DateTime RectificationEndDate { get; set; }
        public DateTime RectificationStartDate { get; set; }
        public bool IsSummer { get; set; } = false;

        [Required]
        public decimal MinGrade { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        //[Required]
        [StringLength(50)]
        public string Number { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public string ResolutionFile { get; set; }

        [Required]
        [StringLength(50)]
        public string ResolutionNumber { get; set; }
        public int Status { get; set; } = 0;

        [Required]
        public int Year { get; set; }
        
        [NotMapped]
        public bool IsActive => Status == ConstantHelpers.TERM_STATES.ACTIVE;

        public ICollection<AcademicHistory> AcademicHistories { get; set; }
        public ICollection<AcademicSummary> AcademicSummaries { get; set; }
        public ICollection<CourseTerm> CourseTerms { get; set; }
        public ICollection<Student> StudentsAdmissionTerm { get; set; }
        public ICollection<Student> StudentsGraduationTerm { get; set; }
        public ICollection<TutorialStudent> TutorialStudents { get; set; }
        public ICollection<UserExternalProcedure> UserExternalProcedures { get; set; }
        public ICollection<InstitutionalWelfareAnswerByStudent> InstitutionalWelfareAnswerByStudents { get; set; }
        public ICollection<PerformanceEvaluation> PerformanceEvaluations { get; set; }
        public ICollection<ScaleExtraPerformanceEvaluationField> ScaleExtraPerformanceEvaluationFields { get; set; }

    }
}
