using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class UserCurrentAccount
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid CurrentAccountId { get; set; }

        public CurrentAccount CurrentAccount { get; set; }

        public bool IsExpenseAccount { get; set; }
    }
}
