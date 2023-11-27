using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerLaborInformation : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? AcademicDepartmentId { get; set; }
        public Guid? BirthCountryId { get; set; }
        public Guid? BirthDepartmentId { get; set; }
        public Guid? BirthDistrictId { get; set; }
        public Guid? BirthProvinceId { get; set; }
        public Guid? BuildingId { get; set; }
        public Guid? CampusId { get; set; }
        public Guid? ResidenceCountryId { get; set; }
        public Guid? ResidenceDepartmentId { get; set; }
        public Guid? ResidenceDistrictId { get; set; }
        public Guid? ResidenceProvinceId { get; set; }

        [Required]
        public string UserId { get; set; }
        public Guid? WorkerCapPositionId { get; set; }
        public Guid? WorkerLaborCategoryId { get; set; }
        public Guid? WorkerLaborConditionId { get; set; }
        public Guid? WorkerLaborRegimeId { get; set; }
        public Guid? WorkerManagementPositionId { get; set; }
        public Guid? WorkerPositionClassificationId { get; set; }
        public int? MaxStudyLevel { get; set; } // AKDEMIC.CORE.Helpers.ConstantHelpers.STUDIES_LEVEL.VALUES



        public string BloodFile { get; set; }
        public byte? BloodType { get; set; }
        public byte? CivilStatus { get; set; }
        public int? DiscapacityCarnetType { get; set; } //Constante
        public string DrivingLicense { get; set; }
        public int? DrivingLicenseCategory { get; set; } //Constante
        public DateTime? EntryDate { get; set; }
        public string EntryFile { get; set; }

        // Family Information
        public bool HasDiscapacity { get; set; }
        public string HomeNumber { get; set; }
        public bool IsUnionized { get; set; } //Sindicalizado
        public byte? LaborType { get; set; }
        public DateTime? LastPdfDownloadDate { get; set; }

        // Family Information

        public string MarriageCertificateNumber { get; set; }
        public DateTime? MarriageDate { get; set; }

        //Family Information
        public string BeneficiaryStatementFile { get; set; }
        public string BeneficiaryStatementDescription { get; set; }



        public string Neighborhood { get; set; } //Barrio
        public string PhysicalFilerCode { get; set; }

        [StringLength(50)]
        public string PlaceCode { get; set; }
        public int ResidenceType { get; set; }

        [StringLength(11)]
        public string Ruc { get; set; }

        // Additional Information

        public string FacebookUrl { get; set; }
        public string LinkedinUrl { get; set; }
        public string TwitterUrl { get; set; }

        // Family Information

        public DateTime? SpouseBirthDate { get; set; }
        public string SpouseEmail { get; set; }
        public string SpouseLeDni { get; set; }
        public string SpouseMaternalName { get; set; }
        public string SpouseName { get; set; }
        public string SpouseOcupation { get; set; }
        public string SpousePaternalName { get; set; }
        public string SpousePhoneNumber { get; set; }
        public string SpouseWorkPlace { get; set; }

        public string UnionName { get; set; } //Nombre del Sindicato

        //Tab de Investigacion
        public bool HasRenacyt { get; set; }
        public string RenacytCode { get; set; }
        public bool HasConcytec { get; set; }
        public string ConcytecCode { get; set; }

        [StringLength(50)]
        public string WorkerCode { get; set; }
        public int? WorkerStatus { get; set; }

        [NotMapped]
        public string EntryFormattedDate { get; set; }
        
        [NotMapped]
        public string LaborTypeString { get; set; }

        [NotMapped]
        public string ResidenceDepartmentString { get; set; }

        [NotMapped]
        public string ResidenceDistrictString { get; set; }

        [NotMapped]
        public string ResidenceProvinceString { get; set; }

        [NotMapped]
        public string ResidenceTypeString => ConstantHelpers.RESIDENCETYPE.VALUES.ContainsKey(ResidenceType) ? ConstantHelpers.RESIDENCETYPE.VALUES[ResidenceType] : "Desconocido";


        public AcademicDepartment AcademicDepartment { get; set; }
        public ApplicationUser User { get; set; }
        public Building Building { get; set; }
        public Campus Campus { get; set; }
        public WorkerCapPosition WorkerCapPosition { get; set; }
        public WorkerManagementPosition WorkerManagementPosition { get; set; }
        public WorkerLaborCategory WorkerLaborCategory { get; set; }
        public WorkerLaborCondition WorkerLaborCondition { get; set; }
        public WorkerLaborRegime WorkerLaborRegime { get; set; }
        public WorkerPositionClassification WorkerPositionClassification { get; set; }

        [InverseProperty("WorkersLaborInformationBirthCountry")]
        public Country BirthCountry { get; set; }

        [InverseProperty("WorkersLaborInformationBirthDepartment")]
        public Department BirthDepartment { get; set; }

        [InverseProperty("WorkersLaborInformationBirthDistrict")]
        public District BirthDistrict { get; set; }

        [InverseProperty("WorkersLaborInformationBirthProvince")]
        public Province BirthProvince { get; set; }

        [InverseProperty("WorkersLaborInformationResidenceCountry")]
        public Country ResidenceCountry { get; set; }

        [InverseProperty("WorkersLaborInformationResidenceDepartment")]
        public Department ResidenceDepartment { get; set; }

        [InverseProperty("WorkersLaborInformationResidenceDistrict")]
        public District ResidenceDistrict { get; set; }

        [InverseProperty("WorkersLaborInformationResidenceProvince")]
        public Province ResidenceProvince { get; set; }

        public ICollection<WorkerBankAccountInformation> WorkerBankAccountInformations { get; set; }
        public ICollection<WorkerFamilyInformation> WorkerFamilyInformations { get; set; }
    }
}
