using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Intranet;
using AKDEMIC.ENTITIES.Models.Investigation;
using AKDEMIC.ENTITIES.Models.JobExchange;
using AKDEMIC.ENTITIES.Models.Laurassia;
using AKDEMIC.ENTITIES.Models.Payroll;
using AKDEMIC.ENTITIES.Models.Preuniversitary;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using AKDEMIC.ENTITIES.Models.TeachingManagement;
using AKDEMIC.ENTITIES.Models.Tutoring;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class ApplicationUser : IdentityUserEntity, IKeyNumber, ISoftDelete, ITimestamp
    {
        public ApplicationUser ShallowCopy()
        {
            return (ApplicationUser)this.MemberwiseClone();
        }

        public Guid? DepartmentId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? ProvinceId { get; set; }
        public string Ubigeo { get; set; }
        public string Address { get; set; }
        public string AllowedSystem { get; set; }

        [MaxLength(500)]
        public string AnswerSecretQuestion { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber2 { get; set; }

        [MaxLength(250)]
        public string Dni { get; set; }

        [MaxLength(200)]
        public string Document { get; set; }
        public byte DocumentType { get; set; } = ConstantHelpers.DOCUMENT_TYPES.DNI;
        public byte CivilStatus { get; set; } = ConstantHelpers.CIVIL_STATUS.SINGLE;
        public bool? FirstTime { get; set; } = true;
        public DateTime? PasswordChangeDate { get; set; }
        public string FullName { get; set; } // = $"{(String.IsNullOrEmpty(PaternalSurname) ? "" : $"{PaternalSurname} ")}{(String.IsNullOrEmpty(MaternalSurname) ? "" : $"{MaternalSurname}")}, {(String.IsNullOrEmpty(Name) ? "" : $"{Name}")}";
        public bool IsActive { get; set; } = true;
        public bool IsLockedOut { get; set; } = false;
        public string LockedOutReason { get; set; }
        public string MysqlPasswordHash { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string MaternalSurname { get; set; }

        [MaxLength(250)]
        public string PaternalSurname { get; set; }
        public string PasswordHint { get; set; }
        public string PersonalEmail { get; set; }
        public string Picture { get; set; }

        [Required]
        public int Sex { get; set; } = 1;
        public int Type { get; set; } = ConstantHelpers.USER_TYPES.NOT_ASIGNED;
        public string UserWeb { get; set; }
        public int State { get; set; } = ConstantHelpers.USER_STATES.ACTIVE;

        [NotMapped]
        public string StateString { get; set; }

        [NotMapped]
        public int Age => BirthDate > DateTime.Today.AddYears(-(DateTime.Today.Year - BirthDate.Year)) ? (DateTime.Today.Year - BirthDate.Year - 1) : DateTime.Today.Year - BirthDate.Year;

        [NotMapped]
        public string BirthDateString => BirthDate.ToLocalDateFormat();

        [NotMapped]
        public string ConcatenatedRoles { get; set; }

        [NotMapped]
        public bool HasBachellor { get; set; }

        [NotMapped]
        public string RawFullName => $"{(string.IsNullOrEmpty(Name) ? "" : $"{Name} ")}{(string.IsNullOrEmpty(PaternalSurname) ? "" : $"{PaternalSurname} ")}{(string.IsNullOrEmpty(MaternalSurname) ? "" : $"{MaternalSurname} ")}";

        [NotMapped]
        public byte? RetirementSystem { get; set; }

        public Department Department { get; set; }
        public District District { get; set; }
        public Province Province { get; set; }
        //public Teacher Teacher { get; set; }
        public Worker Worker { get; set; }
        public WorkerLaborInformation WorkerLaborInformation { get; set; }
        public WorkerPersonalDocument WorkerPersonalDocuments { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public ICollection<WorkerLaborTermInformation> WorkerLaborTermInformations { get; set; }
        public ICollection<AcademicRecordDepartment> AcademicRecordDepartments { get; set; }
        public ICollection<Campaign> Campaigns { get; set; }
        public ICollection<CashierDependency> CashierDependencies { get; set; }
        public ICollection<Connection> Connections { get; set; }
        //public ICollection<FavoriteCompany> FavoriteCompanies { get; set; }
        public ICollection<IncubationCall> IncubationCalls { get; set; }
        //public ICollection<Login> Logins { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<PreuniversitaryUserGroup> PreuniversitaryUserGroups { get; set; }
        public ICollection<ScaleLicenseAuthorization> ScaleLicenseAuthorizations { get; set; }
        public ICollection<ScaleResolution> ScaleResolutions { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
        public ICollection<TeacherSchedule> TeacherSchedules { get; set; }
        public ICollection<Topic> Topics { get; set; }
        public ICollection<Tutor> Tutors { get; set; }
        public ICollection<Tutorial> Tutorials { get; set; }
        public ICollection<UserDependency> UserDependencies { get; set; }
        public ICollection<UserProcedure> UserProcedures { get; set; }
        public ICollection<UserResearchArea> UserResearchAreas { get; set; }
        public ICollection<UserResearchLine> UserResearchLines { get; set; }
        public ICollection<WorkerBachelorDegree> WorkerBachelorDegrees { get; set; }
        public ICollection<WorkerDiplomate> WorkerDiplomates { get; set; }
        public ICollection<WorkerDoctoralDegree> WorkerDoctoralDegrees { get; set; }
        public ICollection<WorkerMasterDegree> WorkerMasterDegrees { get; set; }
        public ICollection<WorkerOtherStudy> WorkerOtherStudies { get; set; }
        public ICollection<WorkerProfessionalSchool> WorkerProfessionalSchools { get; set; }
        public ICollection<WorkerProfessionalTitle> WorkerProfessionalTitles { get; set; }
        public ICollection<WorkerRetirementSystemHistory> WorkerRetirementSystemHistories { get; set; }
        public ICollection<WorkerSecondSpecialty> WorkerSecondSpecialties { get; set; }
        public ICollection<WorkerSchoolDegree> WorkerSchoolDegrees { get; set; }
        public ICollection<WorkerTechnicalStudy> WorkerTechnicalStudies { get; set; }
        public ICollection<WorkerTraining> WorkerTrainings { get; set; }
        public ICollection<WorkingDay> WorkingDays { get; set; }
        public ICollection<UserLogin> UserLogins { get; set; }        
        public ICollection<MedicalAppointment> DoctorMedicalAppointments { get; set; }
        public ICollection<MedicalAppointment> UserMedicalAppointments { get; set; }
        public ICollection<PerformanceEvaluationUser> PerformanceEvaluationUsers { get; set; }
        public ICollection<FavoriteCompany> FavoriteCompanies { get; set; }

    }
}
