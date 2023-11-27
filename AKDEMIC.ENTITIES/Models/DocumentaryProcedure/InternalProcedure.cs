using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class InternalProcedure : Entity, ICodeNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid DependencyId { get; set; }
        public Guid DocumentTypeId { get; set; }
        public Guid? InternalProcedureParentId { get; set; }
        public string UserId { get; set; }
        public bool FromExternal { get; set; } = false;
        public string Content { get; set; }
        public int AnswerType { get; set; }
        public bool IsTransparency { get; set; } = false;

        [Required]
        public int Number { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Pages { get; set; }

        [Required]
        public int Priority { get; set; } = 2;

        public int SearchNode { get; set; }
        public int SearchTree { get; set; }

        [Required]
        public string Subject { get; set; }

        public string Code { get; set; }
        
        [NotMapped]
        public string LegacyCode => $"INTE-{DocumentType?.Code.ToUpper()}-{Number}-{(CreatedAt.HasValue ? CreatedAt.Value.Year.ToString() : null)}-{GeneralHelpers.GetInstitutionAbbreviation()}{(Dependency != null ? "-" : null)}{Dependency?.Acronym.ToUpper()}";

        [NotMapped]
        public bool HasFiles { get; set; }

        [NotMapped]
        public bool HasReferences { get; set; }
        public Guid InternProcedureAnswerd { get; set; }
        public Dependency Dependency { get; set; }
        public DocumentType DocumentType { get; set; }
        public InternalProcedure InternalProcedureParent { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<InternalProcedureFile> InternalProcedureFiles { get; set; }
        public ICollection<InternalProcedureReference> InternalProcedureReferences { get; set; }
        public ICollection<UserExternalProcedure> UserExternalProcedures { get; set; }
        public ICollection<UserInternalProcedure> UserInternalProcedures { get; set; }

        [NotMapped]
        public List<InternalProcedure> InternalProceduresTest { get; set; }
    }
}
