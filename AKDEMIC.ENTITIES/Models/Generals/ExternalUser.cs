using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using AKDEMIC.ENTITIES.Models.EconomicManagement;

namespace AKDEMIC.ENTITIES.Models.Generals
{
    public class ExternalUser : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? DocumentType { get; set; }
        [Required]
        [StringLength(200)]
        public string DocumentNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberOptional { get; set; }
        public string WorkPosition { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string BusinessName { get; set; }

        public string UserId { get; set; }
        public bool IsPublicSector { get; set; }

        [NotMapped]
        public string BirthDateString => BirthDate.ToLocalDateFormat();
        //[NotMapped]
        public string FullName { get; set; } // => PaternalSurname == null && MaternalSurname == null ? $"{Name}" : MaternalSurname == null ? $"{PaternalSurname}, {Name}" : PaternalSurname == null ? $"{MaternalSurname}, {Name}" : $"{PaternalSurname} {MaternalSurname}, {Name}";
        [NotMapped]
        public string RawFullName => $"{Name} {PaternalSurname} {MaternalSurname}";
        [NotMapped]
        public bool HasRelatedUserExternalProcedures { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<UserExternalProcedure> UserExternalProcedures { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
