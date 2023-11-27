using System;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AKDEMIC.ENTITIES.Base.Implementations
{
    public class IdentityUserEntity : IdentityUser, IEntity
    {
        [NotMapped]
        public int GeneratedId { get; set; }

        [NotMapped]
        public string RelationId { get; set; }

        [NotMapped]
        public DateTime? DeletedAt { get; set; }

        [NotMapped]
        public string DeletedBy { get; set; }

        [NotMapped]
        public DateTime? CreatedAt { get; set; }

        [NotMapped]
        public string CreatedBy { get; set; }

        [NotMapped]
        public DateTime? UpdatedAt { get; set; }

        [NotMapped]
        public string UpdatedBy { get; set; }

        [NotMapped]
        public string ParsedDeletedAt => DeletedAt?.ToLocalDateTimeFormat();

        [NotMapped]
        public string ParsedCreatedAt => CreatedAt?.ToLocalDateTimeFormat();

        [NotMapped]
        public string ParsedUpdatedAt => UpdatedAt?.ToLocalDateTimeFormat();

        [NotMapped]
        public string SearchId { get; set; }
    }
}
