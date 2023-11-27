using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class Procedure : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? ConceptId { get; set; }
        public Guid? ClassifierId { get; set; }
        public Guid? DependencyId { get; set; }
        public Guid? ProcedureCategoryId { get; set; }
        public Guid? ProcedureSubcategoryId { get; set; }
        public Guid? StartDependencyId { get; set; }

        public bool Enabled { get; set; }
        public byte Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public byte? MaximumRequestByTerm { get; set; }

        public DateTime? EnabledStartDate { get; set; }
        public DateTime? EnabledEndDate { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Duration { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Score { get; set; }
        public string LegalBase { get; set; }
        public int? StaticType { get; set; }

        [NotMapped]
        public decimal ProcedureRequirementsCostSum { get; set; }

        public bool HasPicture { get; set; } 
        public Concept Concept { get; set; }
        public Classifier Classifier { get; set; }
        public Dependency Dependency { get; set; }
        public Dependency StartDependency { get; set; }
        public ProcedureCategory ProcedureCategory { get; set; }
        public ProcedureSubcategory ProcedureSubcategory { get; set; }
        public ICollection<ProcedureDependency> ProcedureDependencies { get; set; }
        public ICollection<ProcedureRequirement> ProcedureRequirements { get; set; }
        public ICollection<ProcedureResolution> ProcedureResolutions { get; set; }
        public ICollection<ProcedureRole> ProcedureRoles { get; set; }
        public ICollection<UserProcedure> UserProcedures { get; set; }
        public ICollection<ProcedureTask> ProcedureTasks { get; set; }
        public ICollection<ProcedureAdmissionType> ProcedureAdmissionTypes { get; set; }
    }
}
