﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerOtherStudy : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Specialty { get; set; }

        public Guid InstitutionId { get; set; }
        public Institution Institution { get; set; }

        [Required]
        public byte InstitutionType { get; set; }

        public int? Hours { get; set; }

        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        public DateTime ExpeditionDate { get; set; }

        [NotMapped]
        public string ExpeditionFormattedDate { get; set; }

        [Required]
        public string StudyDocument { get; set; }

        [Required]
        public bool IsApprovedBySunedu { get; set; }

        [Required]
        public Guid StudyCountryId { get; set; }
        public Guid? StudyDepartmentId { get; set; }
        public Guid? StudyProvinceId { get; set; }
        public Guid? StudyDistrictId { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
