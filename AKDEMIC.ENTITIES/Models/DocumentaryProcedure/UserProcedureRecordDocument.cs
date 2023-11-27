using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;

namespace AKDEMIC.ENTITIES.Models.DocumentaryProcedure
{
    public class UserProcedureRecordDocument : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public long DocumentBytesSize { get; set; }

        [Required]
        public string DocumentUrl { get; set; }

        public Guid UserProcedureRecordId { get; set; }
        public UserProcedureRecord UserProcedureRecord { get; set; }
    }
}
