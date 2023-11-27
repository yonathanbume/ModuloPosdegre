using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class Requirement : Entity, ICodeNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? SupplierId { get; set; }
        public Guid? PhaseRequirementId { get; set; }
        public Guid? DependencyId { get; set; }
        public string UserId { get; set; }
        public byte Type { get; set; }
        public string Description { get; set; }
        public int Folio { get; set; }
        public string Need { get; set; }
        [Required]
        public string CodeNumber { get; set; }
        public string Subject { get; set; }

        [NotMapped]
        public string Code => $"REQ-";

        public ApplicationUser User { get; set; }
        public Supplier Supplier { get; set; }
        public PhaseRequirement PhaseRequirement { get; set; }
        public Dependency Dependency { get; set; }
        public ICollection<UserRequirement> UserRequirements { get; set; }
        public ICollection<RequirementFile> RequirementFiles { get; set; }
        public ICollection<RequirementSupplier> RequirementSuppliers { get; set; }
    }
}
