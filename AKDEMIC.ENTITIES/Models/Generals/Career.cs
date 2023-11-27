using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class Career : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public string AcademicCoordinatorId { get; set; }
        public string AcademicDepartmentDirectorId { get; set; }
        public string AcademicSecretaryId { get; set; }
        public string CareerDirectorId { get; set; }
        public Guid FacultyId { get; set; }
        public string QualityCoordinatorId { get; set; }
        public string ResearchCoordinatorId { get; set; }
        public string SocialResponsabilityCoordinatorId { get; set; }
        public string InternshipCoordinatorId { get; set; }
        public DateTime? CareerDirectorUpdate { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        //[StringLength(5000)]
        public string Comments { get; set; }

        //[StringLength(5000)]
        public string GeneralInformation { get; set; }

        //[StringLength(5000)]
        public string GraduateProfile { get; set; }
        public bool IsActive { get; set; } = true;

        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        public DateTime? DecanalResolutionDate { get; set; }
        public string DecanalResolutionFile { get; set; }
        public string DecanalResolutionNumber { get; set; }
        public DateTime? RectoralResolutionDate { get; set; }
        public string RectoralResolutionFile { get; set; }
        public string RectoralResolutionNumber { get; set; }

        //public Guid SchoolId { get; set; }
        //public School School { get; set; }

        public ApplicationUser AcademicCoordinator { get; set; }
        public ApplicationUser AcademicSecretary { get; set; }
        public ApplicationUser AcademicDepartmentDirector { get; set; }
        public ApplicationUser CareerDirector { get; set; }
        public ApplicationUser ResearchCoordinator { get; set; }
        public ApplicationUser SocialResponsabilityCoordinator { get; set; }
        public ApplicationUser QualityCoordinator { get; set; }
        public ApplicationUser InternshipCoordinator { get; set; }
        public Faculty Faculty { get; set; }

        public ICollection<AcademicProgram> AcademicPrograms { get; set; }
        public ICollection<AcademicDepartment> AcademicDepartments { get; set; }
        public ICollection<AcademicSummary> AcademicSummaries { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Curriculum> Curriculums { get; set; }
        public ICollection<EntrantEnrollment> EntrantEnrollments { get; set; }
        public ICollection<InterestGroup.InterestGroup> InterestGroups { get; set; }
        public ICollection<Postulant> Postulants { get; set; }
        public ICollection<PreuniversitaryCourse> PreuniversitaryCourses { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Tutor> Tutors { get; set; }
        public ICollection<TutoringAnnouncementCareer> TutoringAnnouncementCareers { get; set; }
        public ICollection<CareerApplicationTerm> CareerApplicationTerms { get; set; }
        public ICollection<CareerHistory> CareerHistories { get; set; }
        public ICollection<CareerAccreditation> CareerAccreditations { get; set; }
        public ICollection<CoordinatorCareer> CoordinatorCareers { get; set; }
    }
}
