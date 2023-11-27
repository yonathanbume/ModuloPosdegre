using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class PettyCash : Entity, ITimestamp
    {
        public Guid Id { get; set; }
        //public Guid SerialNumberId { get; set; }
        public string UserId { get; set; }
        public DateTime InitialDate { get; set; }

        [Required]
        public decimal InitialAmount { get; set; }
        public decimal DeclaredAmount { get; set; }
        public decimal AmountCollected { get; set; }
        public bool Closed { get; set; } = false;

        public ApplicationUser User { get; set; }
        //public SerialNumber SerialNumber { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
    }
}
