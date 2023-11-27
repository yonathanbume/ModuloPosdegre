using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class PettyCashBook
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<BankDeposit> BankDeposits { get; set; }
    }
}
