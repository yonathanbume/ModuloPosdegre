using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class RecordSubjectType : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }
        
        [Required]
        public string Name { get; set; }

        [NotMapped]
        public bool HasRelatedUserProcedures { get; set; }

        public ICollection<UserExternalProcedureRecord> UserExternalProcedureRecords { get; set; }
        public ICollection<UserProcedureRecord> UserProcedureRecords { get; set; }
    }
}
