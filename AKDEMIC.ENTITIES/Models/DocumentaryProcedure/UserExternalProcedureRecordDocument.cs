using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class UserExternalProcedureRecordDocument : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public long DocumentBytesSize { get; set; }

        [Required]
        public string DocumentUrl { get; set; }

        public Guid UserExternalProcedureRecordId { get; set; }
        public UserExternalProcedureRecord UserExternalProcedureRecord { get; set; }
    }
}
