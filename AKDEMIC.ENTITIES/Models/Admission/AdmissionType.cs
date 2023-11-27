using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class AdmissionType : Entity, IKeyNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? PrivateSchoolConceptId { get; set; }
        public Guid? PublicSchoolConceptId { get; set; }
        public Guid? ForeignSchoolConceptId { get; set; }

        [Required]
        [StringLength(50)]
        public string Abbreviation { get; set; }
        //public decimal Cost { get; set; } = 0;
        public bool IsExonerated { get; set; } = false;
        public bool IsExoneratedEnrollment { get; set; } = false;
        public bool IsPermanent { get; set; } = true;
        //public bool IsRegular { get; set; }
        public bool IsTransfer { get; set; }
        public bool HasDisability { get; set; }

        public bool IsGraduateAdmissionType { get; set; }
        public bool IsCepreAdmissionType { get; set; }
        public bool IsInternalTransfer { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Resolution { get; set; }
        public DateTime ResolutionDate { get; set; }
        public string ResolutionFile { get; set; }
        public byte Type { get; set; } = 0;
        public DateTime? ValidityEnd { get; set; }
        public DateTime? ValidityStart { get; set; }

        public Concept PrivateSchoolConcept { get; set; }
        public Concept PublicSchoolConcept { get; set; }
        public Concept ForeignSchoolConcept { get; set; }

        public ICollection<AdmissionRequirement> Requirementes { get; set; }
        public ICollection<AdmissionTypeDescount> AdmissionTypeDescounts { get; set; }
        public ICollection<ApplicationTermAdmissionType> ApplicationTermAdmissionTypes { get; set; }
        public ICollection<Postulant> Postulants { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<ProcedureAdmissionType> ProcedureAdmissionTypes { get; set; }
    }
}