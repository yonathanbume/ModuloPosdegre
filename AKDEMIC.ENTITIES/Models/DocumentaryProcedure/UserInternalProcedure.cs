using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class UserInternalProcedure : Entity, ICodeNumber, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid DependencyId { get; set; }
        public Guid DependencyParentId { get; set; }
        public Guid InternalProcedureId { get; set; }
        public string UserId { get; set; }

        public Guid? ProcedureFolderId { get; set; }

        public string Observation { get; set; }
        public DateTime? FinishAt { get; set; }
        [Required]
        public bool IsDerived { get; set; } = false;

        [Required]
        public int Status { get; set; } = 1;

        public string FinalUrl { get; set; }

        [Range(0, int.MaxValue)]
        public int Duration { get; set; } = 0;
        [NotMapped]
        public Dependency DependencyParent { get; set; }
        public Dependency Dependency { get; set; }
        public InternalProcedure InternalProcedure { get; set; }
        public ApplicationUser User { get; set; }
        public ProcedureFolder ProcedureFolder { get; set; }
        [NotMapped]
        public string ParsedFinishAt { get; set; }

        [NotMapped]
        public string StatusStr { get; set; }

        [NotMapped]
        public string UserExternalProcedureCode { get; set; }
    }
}
