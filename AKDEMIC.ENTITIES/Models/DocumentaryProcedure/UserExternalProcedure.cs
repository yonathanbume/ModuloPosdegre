using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Enrollment;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class UserExternalProcedure : Entity, ICodeNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid DependencyId { get; set; }
        public Guid ExternalProcedureId { get; set; }
        public Guid ExternalUserId { get; set; }
        public Guid? InternalProcedureId { get; set; }
        public Guid? PaymentId { get; set; }
        public Guid? TermId { get; set; }

        [Required]
        public int Number { get; set; } = 1;
        public string Observation { get; set; }

        [Required]
        public int Status { get; set; } = 1;

        [NotMapped]
        public string Code { get; set; }

        public Dependency Dependency { get; set; }
        public ExternalProcedure ExternalProcedure { get; set; }
        public ExternalUser ExternalUser { get; set; }
        public InternalProcedure InternalProcedure { get; set; }
        public Payment Payment { get; set; }
        public Term Term { get; set; }
        public ICollection<UserExternalProcedureFile> UserExternalProcedureFiles { get; set; }
        public ICollection<UserExternalProcedureRecord> UserExternalProcedureRecords { get; set; }
    }
}
