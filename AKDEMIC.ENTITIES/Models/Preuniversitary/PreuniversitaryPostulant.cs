using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Preuniversitary
{
    public class PreuniversitaryPostulant
    {
        public Guid Id { get; set; }

        public Guid? PreuniversitaryTermId { get; set; }

        public Guid BirthCountryId { get; set; }
        public Guid? BirthDepartmentId { get; set; }
        public Guid? BirthDistrictId { get; set; }
        public Guid? BirthProvinceId { get; set; }

        public Guid DepartmentId { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid DistrictId { get; set; }

        public Guid NationalityCountryId { get; set; }

        public string Code { get; set; }
        public string WebCode { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string Document { get; set; }

        public int Sex { get; set; } = ConstantHelpers.SEX.MALE;
        public DateTime BirthDate { get; set; }
        public int MaritalStatus { get; set; } = ConstantHelpers.MARITAL_STATUS.SINGLE;

        public string NativeLanguage { get; set; }
        public string SecondLanguage { get; set; }

        [Required]
        public string Address { get; set; }
        public string AddressReference { get; set; }
        public string ZoneType { get; set; }
        public string Zone { get; set; }
        public string RoadType { get; set; }
        public string Road { get; set; }

        [Phone]
        public string Phone { get; set; }
        [Phone]
        public string Cellphone1 { get; set; }
        [Phone]
        public string Cellphone2 { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        public int Representative { get; set; } = 1;
        public string RepresentativeName { get; set; }
        public string RepresentativeRelation { get; set; }
        [Phone]
        public string RepresentativePhone { get; set; }
        [Phone]
        public string RepresentativeCellphone { get; set; }
        [EmailAddress]
        public string RepresentativeEmail { get; set; }
        public string RepresentativeAddress { get; set; }
        public string RepresentativeDocument { get; set; }

        [Required]
        public int BroadcastMedium { get; set; } = ConstantHelpers.BROADCAST_MEDIUM.INTERNET;
        public string BroadcastMediumOther { get; set; }

        public string Picture { get; set; }
        public DateTime RegisterDate { get; set; }

        public string Observations { get; set; }

        public PreuniversitaryTerm PreuniversitaryTerm { get; set; }

        public string Preparation { get; set; }
        public string Postulation { get; set; }
        public string PostulantSituation { get; set; }
        public byte AdmissionState { get; set; } = 0; //0 Pendiente , 1 Admitido , 2 No admitido

        public Guid PreuniversitaryChannelId { get; set; }
        public PreuniversitaryChannel PreuniversitaryChannel { get; set; }

        public Guid CareerId { get; set; }
        public Career Career { get; set; }

        public string AdmissionType { get; set; }
        
        [Required]
        public int SecondaryEducationType { get; set; } = 1;
        public string SecondaryEducationTypeOther { get; set; }

        [Required]
        public string SecondaryEducationName { get; set; }
        public string SecondaryEducationAddress { get; set; }

        [Required]
        public int SecondaryEducationFinished { get; set; } = 1;
        public string SecondaryEducationFinishedOther { get; set; }

        [Required]
        public DateTime SecondaryEducationStarts { get; set; }
        public DateTime? SecondaryEducationEnds { get; set; }

        public Guid? SecondaryEducationSchoolId { get; set; }
        public School SecondaryEducationSchool { get; set; }

        public bool IsPaid { get; set; } = false;
        public string PaymentType { get; set; }
        public string PaymentConcept { get; set; }
        public string PaymentInvoice { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }

        public int OrderMerit { get; set; } = -1;
        public decimal FinalScore { get; set; } = 0.00M;

        [InverseProperty("PreUniversityPostulantsBirthCountry")]
        public Country BirthCountry { get; set; }

        [InverseProperty("PreUniversityPostulantsNationalityCountry")]
        public Country NationalityCountry { get; set; }

        [InverseProperty("PreUniversityPostulantsBirthDepartment")]
        public Department BirthDepartment { get; set; }

        [InverseProperty("PreUniversityPostulantsDepartment")]
        public Department Department { get; set; }

        [InverseProperty("PreUniversityPostulantsBirthDistrict")]
        public District BirthDistrict { get; set; }

        [InverseProperty("PreUniversityPostulantsDistrict")]
        public District District { get; set; }

        [InverseProperty("PreUniversityPostulantsBirthProvince")]
        public Province BirthProvince { get; set; }

        [InverseProperty("PreUniversityPostulantsProvince")]
        public Province Province { get; set; }

    }
}
