using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerDoctoralDegree : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid InstitutionId { get; set; }
        public Guid StudyCountryId { get; set; }
        public Country StudyCountry { get; set; }
        public Guid? StudyDepartmentId { get; set; }
        public Department StudyDepartment { get; set; }
        public Guid? StudyDistrictId { get; set; }
        public District StudyDistrict { get; set; }
        public Guid? StudyProvinceId { get; set; }
        public Province StudyProvince { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        public DateTime ExpeditionDate { get; set; }

        [Required]
        public byte InstitutionType { get; set; }

        [Required]
        public bool IsApprovedBySunedu { get; set; }
        public bool IsComplementary { get; set; }

        [Required]
        [StringLength(900)]
        public string Specialty { get; set; }
        
        [Required]
        public string StudyDocument { get; set; }
        public string StudyValidationDocument { get; set; }
        public string ValidationText { get; set; }

        public int FolioNumber { get; set; } //Numero de registro de hojas
        public string BookNumber { get; set; } //Numero de Libro
        public string DiplomaNumber { get; set; } // código o numero de diploma

        [NotMapped]
        public string ExpeditionFormattedDate { get; set; }

        public ApplicationUser User { get; set; }
        public Institution Institution { get; set; }
    }
}
