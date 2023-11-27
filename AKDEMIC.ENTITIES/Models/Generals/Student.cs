using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.AcademicExchange;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Degree;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class Student : Entity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? AcademicAgreementId { get; set; }
        public Guid? AcademicProgramId { get; set; }
        public Guid? SpecialtyId { get; set; }
        public Guid AdmissionTermId { get; set; }
        public Guid AdmissionTypeId { get; set; }
        public Guid? CampusId { get; set; }
        public Guid CareerId { get; set; }
        public Guid CurriculumId { get; set; }
        public Guid? GraduationTermId { get; set; }
        public Guid? PsychologicalRecordId { get; set; }
        public Guid? MedicalRecordId { get; set; }
        public Guid? ScholarshipId { get; set; } //Beca Asociada
        public string UserId { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public Guid? FirstEnrollmentTermId { get; set; }
        public DateTime? FirstEnrollmentDate { get; set; }
        public byte CareerNumber { get; set; }
        public int CurrentAcademicYear { get; set; } = 1;
        public int CurrentMeritOrder { get; set; } = -1;
        public int CurrentMeritType { get; set; } = ConstantHelpers.ACADEMIC_ORDER.NONE;
        //public decimal LastWeightedAverageGrade { get; set; } = -1;
        public int PromotionalOrder { get; set; } = -1;
        public DateTime RegisterDate { get; set; } = DateTime.UtcNow;
        public int Status { get; set; } = ConstantHelpers.Student.States.REGULAR; //1 Ingresante - 2 Regular - etc

        public byte RacialIdentity { get; set; } = ConstantHelpers.Student.RacialIdentity.OTHER;
        //public decimal WeightedAverageCumulative { get; set; } = -1;
        public bool HasGraduatedSurveyCompleted { get; set; }

        [NotMapped]
        public AcademicSummary LastAcademicSummary { get; set; }

        [NotMapped]
        public int Position { get; set; }

        [NotMapped]
        public string StatusString => ConstantHelpers.Student.States.VALUES.ContainsKey(Status) ? ConstantHelpers.Student.States.VALUES[Status] : "-";

        [NotMapped]
        public bool StatusIsActive => Status == ConstantHelpers.Student.States.ENTRANT ||
                Status == ConstantHelpers.Student.States.REGULAR ||
                Status == ConstantHelpers.Student.States.TRANSFER ||
                Status == ConstantHelpers.Student.States.IRREGULAR ||
                Status == ConstantHelpers.Student.States.REPEATER ||
                Status == ConstantHelpers.Student.States.UNBEATEN ||
                Status == ConstantHelpers.Student.States.HIGH_PERFORMANCE ||
                Status == ConstantHelpers.Student.States.OBSERVED;

        //public Guid? StudentScaleId { get; set; }
        //public StudentScale StudentScale { get; set; }

        public Guid? EnrollmentFeeId { get; set; }
        public EnrollmentFee EnrollmentFee { get; set; }

        //public Guid? StudentConditionId { get; set; }
        //public StudentCondition StudentCondition { get; set; }
        public byte Condition { get; set; } = ConstantHelpers.Student.Condition.REGULAR;
        public byte InitialBenefit { get; set; } = ConstantHelpers.Student.Benefit.NONE;
        public byte Benefit { get; set; } = ConstantHelpers.Student.Benefit.NONE;
        
        public int? MoodleId { get; set; }

        //public Guid? StudentBenefitId { get; set; }
        //public StudentBenefit StudentBenefit { get; set; }

        public AcademicAgreement AcademicAgreement { get; set; }
        public AcademicProgram AcademicProgram { get; set; }
        public Specialty Specialty { get; set; }
        public AdmissionType AdmissionType { get; set; }
        public ApplicationUser User { get; set; }
        public Campus Campus { get; set; }
        public Career Career { get; set; }
        public Curriculum Curriculum { get; set; }
        public PsychologicalRecord PsychologicalRecord { get; set; }
        public Scholarship Scholarship { get; set; }

        public Guid? StudentInformationId { get; set; }
        //[NotMapped]
        [ForeignKey("StudentInformationId")]
        public StudentInformation StudentInformation { get; set; }

        public Term FirstEnrollmentTerm { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

        [InverseProperty("StudentsAdmissionTerm")]
        public Term AdmissionTerm { get; set; }

        [InverseProperty("StudentsGraduationTerm")]
        public Term GraduationTerm { get; set; }

        public ICollection<AcademicHistory> AcademicHistories { get; set; }
        public ICollection<AcademicSummary> AcademicSummaries { get; set; }
        public ICollection<ClassStudent> ClassStudents { get; set; }
        //public ICollection<Employee> Employees { get; set; }
        public ICollection<EnrollmentTurn> EnrollmentTurns { get; set; }
        public ICollection<EnrollmentReservation> EnrollmentReservations { get; set; }
        public ICollection<Postulant> Postulants { get; set; }
        public ICollection<RecordHistory> RecordHistories { get; set; }
        public ICollection<RegistryPattern> RegistryPatterns { get; set; }
        public ICollection<StudentExperience> StudentExperiences { get; set; }
        public ICollection<StudentGroup> StudentGroups { get; set; }
        public ICollection<StudentObservation> Observations { get; set; }
        public ICollection<StudentSection> StudentSections { get; set; }
        public ICollection<SubstituteExam> SubstituteExams { get; set; }
        public ICollection<TmpEnrollment> TmpEnrollments { get; set; }
        public ICollection<DisapprovedCourse> DisapprovedCourses { get; set; }
        //public ICollection<AppointmentReffered> AppointmentReffereds { get; set; }
        public ICollection<TutoringStudent> TutoringStudents { get; set; }
        public ICollection<StudentFamily> StudentFamilies { get; set; }
        public ICollection<CurriculumVitae> CurriculumVitaes { get; set; }
        public ICollection<InstitutionalWelfareAnswerByStudent> InstitutionalWelfareAnswerByStudents { get; set; }
        
        public ICollection<StudentInformation> StudentInformations { get; set; }
        public ICollection<StudentPortfolio> StudentPortfolios { get; set; }
        public ICollection<StudentAcademicEducation> StudentAcademicEducations { get; set; }
        public ICollection<StudentCertificate> StudentCertificates { get; set; }

    }
}
