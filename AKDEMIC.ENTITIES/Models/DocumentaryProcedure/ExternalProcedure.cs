using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class ExternalProcedure : Entity, ICodeNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? ClassifierId { get; set; }
        public Guid DependencyId { get; set; }
        public Guid? ConceptId { get; set; }
        public string Code { get; set; }
        public string Comment { get; set; }
        public Guid? UITId { get; set; }
        [Required]
        public decimal Cost { get; set; }
        public bool IsTransparency { get; set; } = false;

        [Required]
        public string Name { get; set; }
        public int? StaticType { get; set; }

        public Classifier Classifier { get; set; }
        public Dependency Dependency { get; set; }
        public Concept Concept { get; set; }
        public UIT UIT { get; set; }
        public ICollection<UserExternalProcedure> UserExternalProcedures { get; set; }
    }
}
