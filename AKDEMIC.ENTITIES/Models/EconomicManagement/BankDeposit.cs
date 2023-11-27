using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class BankDeposit
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public Guid CurrentAccountId { get; set; }

        public CurrentAccount CurrentAccount { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public Guid PettyCashBookId { get; set; }

        public PettyCashBook PettyCashBook { get; set; }
    }
}
