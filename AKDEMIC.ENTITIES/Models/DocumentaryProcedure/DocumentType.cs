using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class DocumentType : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public bool HasRelated { get; set; }

        public ICollection<UserExternalProcedureRecord> UserExternalProcedureRecords { get; set; }
        public ICollection<UserProcedureRecord> UserProcedureRecords { get; set; }
    }
}
