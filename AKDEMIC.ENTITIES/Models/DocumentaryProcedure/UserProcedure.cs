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
    public class UserProcedure : Entity, ICodeNumber, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public Guid? DependencyId { get; set; }
        public Guid? PaymentId { get; set; }
        public Guid ProcedureId { get; set; }
        public Guid? TermId { get; set; }
        public Guid? ProcedureFolderId { get; set; }
        public string UserId { get; set; }

        public Guid? RecordHistoryId { get; set; }
        public Guid? StudentUserProcedureId { get; set; }

        [StringLength(8)]
        [RegularExpression("[^0-9]")]
        public string DNI { get; set; }

        [Required]
        public int Number { get; set; } = 1;

        [Required]
        public int Status { get; set; } = 1;
        public string Comment { get; set; }
        public string Observation { get; set; }
        public string ObservationStatus { get; set; }
        public string UrlImage { get; set; }

        public string FinalFileUrl { get; set; }
        public string Correlative { get; set; }

        [NotMapped]
        public string Code => $"TUPA-{Number}-{(CreatedAt.HasValue ? CreatedAt.Value.Year.ToString() : null)}";
        public ApplicationUser User { get; set; }
        public Payment Payment { get; set; }
        public Procedure Procedure { get; set; }
        public Dependency Dependency { get; set; }
        public Term Term { get; set; }
        public ProcedureFolder ProcedureFolder { get; set; }
        public RecordHistory RecordHistory { get; set; }
        public StudentUserProcedure StudentUserProcedure { get; set; }
        public ICollection<EnrollmentReservation> EnrollmentReservations { get; set; }
        public ICollection<UserProcedureDerivation> UserProcedureDerivations { get; set; }
        public ICollection<UserProcedureRecord> UserProcedureRecords { get; set; }
        public ICollection<UserProcedureFile> UserProcedureFiles { get; set; }

        [NotMapped]
        public string CreatedFormattedDate { get; set; }
        [NotMapped]
        public string StatusString { get; set; }

    }
}
