using AKDEMIC.ENTITIES.Models.Payroll;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Scale.Entities
{
    public class WorkerBankAccountInformation
    {
        public Guid Id { get; set; }

        public string CCI { get; set; }
        public string AccountNumber { get; set; }

        public Guid BankId { get; set; }
        public Guid WorkerLaborInformationId { get; set; }

        public Bank Bank { get; set; }
        public WorkerLaborInformation WorkerLaborInformation { get; set; }
    }
}
