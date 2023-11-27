using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class Postulant : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? AcademicProgramId { get; set; }
        public Guid AdmissionTypeId { get; set; }
        public Guid ApplicationTermId { get; set; }
        public Guid BirthCountryId { get; set; }
        public Guid? BirthDepartmentId { get; set; }
        public Guid? BirthDistrictId { get; set; }
        public Guid? BirthProvinceId { get; set; }
        public Guid CampusId { get; set; }
        public Guid? ExamCampusId { get; set; }
        public Guid CareerId { get; set; }
        public Guid? ChannelId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid DistrictId { get; set; }
        public Guid NationalityCountryId { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid? SecondaryEducationDepartmentId { get; set; }
        public Guid? SecondaryEducationDistrictId { get; set; }
        public Guid? SecondaryEducationProvinceId { get; set; }
        public Guid? StudentId { get; set; }

        public Guid? SecondaryEducationSchoolId { get; set; }
        public School SecondaryEducationSchool { get; set; }

        [Required]
        public string Address { get; set; }
        public int? AdmissionFolder { get; set; }
        public byte AdmissionState { get; set; } = 0; //0 Pendiente , 1 Admitido , 2 No admitido

        [Required]
        public DateTime BirthDate { get; set; }
        
        [Required]
        public int BroadcastMedium { get; set; } = 1;
        public string BroadcastMediumOther { get; set; }
        public string Business { get; set; }
        
        [Required]
        public int Childrens { get; set; } = 0;
        public string Code { get; set; }
        public string ControlPhotoPath { get; set; }
        public string CvPostulant { get; set; }
        public string DiscapacityType { get; set; }

        [Required]
        public string Document { get; set; }

        [Required]
        public byte DocumentType { get; set; } = ConstantHelpers.DOCUMENT_TYPES.DNI;

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string EmploymentStatus { get; set; }
        public decimal FinalScore { get; set; } = 0.00M;
        public bool HasDiscapacity { get; set; }
        public bool HasOtherSuperiorEducation { get; set; } = false;
        public bool HasTwoSuperiorEducations { get; set; } = false;
        public bool IsVerified { get; set; }
        public string VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }

        [Required]
        public int MaritalStatus { get; set; } = 1;

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
        [StringLength(255)]
        public string MaternalSurname { get; set; }

        [Required]
        [StringLength(255)]
        public string PaternalSurname { get; set; }
        public string Observations { get; set; }
        public string Occupation { get; set; }
        public int OrderMerit { get; set; } = -1;
        public int OrderMeritBySchool { get; set; } = -1;
        public bool PaidAdmission { get; set; } = false;
        
        [Phone]
        public string Phone1 { get; set; }

        [Phone]
        public string Phone2 { get; set; }
        public string Picture { get; set; }
        public string FingerprintPicture { get; set; }
        public string ReniecPicture { get; set; }

        public string PostulantPhotoPath { get; set; }
        public byte ProofOfIncomeStatus { get; set; } = 0;
        public DateTime RegisterDate { get; set; }

        [Required]
        public int Representative { get; set; } = 1;
        public string RepresentativeBusiness { get; set; }
        
        [EmailAddress]
        public string RepresentativeEmail { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeOcupation { get; set; }

        [Phone]
        public string RepresentativePhone { get; set; }
        public string RepresentativeRelation { get; set; }

        public string SecondaryEducationAddress { get; set; }
        public DateTime? SecondaryEducationEnds { get; set; }

        [Required]
        public int SecondaryEducationFinished { get; set; } = 1;
        public string SecondaryEducationFinishedOther { get; set; }

        [Required]
        public string SecondaryEducationName { get; set; }

        [Required]
        public DateTime SecondaryEducationStarts { get; set; }

        [Required]
        public int SecondaryEducationType { get; set; } = 1;
        public string SecondaryEducationTypeOther { get; set; }

        [Required]
        public int Sex { get; set; } = 1;
        public bool StateObservations { get; set; }

        public string SuperiorEducationInstitution1 { get; set; }
        public bool SuperiorEducationFinished1 { get; set; }
        public DateTime SuperiorEducationStarts1 { get; set; }
        public DateTime? SuperiorEducationEnds1 { get; set; }
        public string SuperiorEducationCareer1 { get; set; }
        public string SuperiorEducationObtainedDegree1 { get; set; }

        public string SuperiorEducationInstitution2 { get; set; }
        public bool SuperiorEducationFinished2 { get; set; }
        public DateTime SuperiorEducationStarts2 { get; set; }
        public DateTime? SuperiorEducationEnds2 { get; set; }
        public string SuperiorEducationCareer2 { get; set; }
        public string SuperiorEducationObtainedDegree2 { get; set; }

        public string Voucher { get; set; }
        public bool WorkingCurrently { get; set; } = false;

        // NUEVAS SECCIONES
        // deportista calificado
        public string Sport { get; set; }
        // traslado externo
        public string OriginUniversity { get; set; }
        public string OriginAcademicProgam { get; set; }
        public string LastEnrolledTermExternal { get; set; }
        public int LastEnrolledYear { get; set; }
        public int ApprovedCredits { get; set; }
        // graduados o titulados
        public string GraduationUniversity { get; set; }
        public string GraduationAcademicProgam { get; set; }
        public string StudyTerm { get; set; }
        public int BachelorOrGraduate { get; set; }
        // trslado interno
        public Guid? InternalAcademicProgram { get; set; }
        public Guid? InternalLastEnrolledTerm { get; set; }
        public decimal InternalApprovedCredits { get; set; }
        // graduado de las ffaa y pnp
        public string GraduationSchool { get; set; }
        public string StudyTermPNP { get; set; }

        //preparacion universitaria
        public int? UniversityPreparationId { get; set; }

        public string OriginalPeople { get; set; }
        public bool IsAfroperuvian { get; set; }
        public bool IsAsianDescendant { get; set; }

        public byte RacialIdentity { get; set; } = ConstantHelpers.Student.RacialIdentity.OTHER;

        public bool IsReinscription { get; set; }

        [NotMapped]
        public bool Accepted => AdmissionState == ConstantHelpers.Admission.Postulant.AdmissionState.ADMITTED;
        
        [NotMapped]
        public int Age => DateTime.UtcNow.Year - BirthDate.Year;

        [NotMapped]
        public string FullName => $"{(string.IsNullOrEmpty(PaternalSurname) ? "" : $"{PaternalSurname} ")}{(string.IsNullOrEmpty(MaternalSurname) ? "" : $"{MaternalSurname}")}, {(string.IsNullOrEmpty(Name) ? "" : $"{Name}")}";

        [NotMapped]
        public string RawFullName => $"{Name} {PaternalSurname} {MaternalSurname}";

        public AcademicProgram AcademicProgram { get; set; }
        public AdmissionType AdmissionType { get; set; }
        public ApplicationTerm ApplicationTerm { get; set; }
        public Campus Campus { get; set; }
        public Campus ExamCampus { get; set; }
        public Career Career { get; set; }
        public Channel Channel { get; set; }
        public Student Student { get; set; }

        [InverseProperty("PostulantsBirthCountry")]
        public Country BirthCountry { get; set; }

        [InverseProperty("PostulantsNationalityCountry")]
        public Country NationalityCountry { get; set; }

        [InverseProperty("PostulantsBirthDepartment")]
        public Department BirthDepartment { get; set; }

        [InverseProperty("PostulantsDepartment")]
        public Department Department { get; set; }

        [InverseProperty("PostulantsSecondaryEducationDepartment")]
        public Department SecondaryEducationDepartment { get; set; }

        [InverseProperty("PostulantsBirthDistrict")]
        public District BirthDistrict { get; set; }

        [InverseProperty("PostulantsDistrict")]
        public District District { get; set; }

        [InverseProperty("PostulantsSecondaryEducationDistrict")]
        public District SecondaryEducationDistrict { get; set; }

        [InverseProperty("PostulantsBirthProvince")]
        public Province BirthProvince { get; set; }

        [InverseProperty("PostulantsProvince")]
        public Province Province { get; set; }

        [InverseProperty("PostulantsSecondaryEducationProvince")]
        public Province SecondaryEducationProvince { get; set; }
        
        public ICollection<AdmissionExamClassroomPostulant> AdmissionExamClassroomPostulants { get; set; }
        public ICollection<ApplicationTermSurveyUser> ApplicationTermSurveyUsers { get; set; }
        public ICollection<PostulantAdmissionRequirement> PostulantAdmissionRequirements { get; set; }
    }
}
