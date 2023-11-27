using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.Scale.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.Payroll
{
    public class Worker : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        // PAYROLL
        public Guid? WorkAreaId { get; set; }
        public WorkArea WorkArea { get; set; }

        public Guid? PayrollTypeId { get; set; }

        public PayrollType PayrollType { get; set; }

        public Guid? ServerTypeId { get; set; }
        [InverseProperty("WorkersServerType")]
        public AdministrativeTable ServerType { get; set; }

        public Guid? SituationId { get; set; }
        [InverseProperty("WorkersSituation")]
        public AdministrativeTable Situation { get; set; }

        public Guid? WorkerOcupationId { get; set; }
        public WorkerOcupation WorkerOcupation { get; set; }

        public DateTime? RetirementInscriptionDate { get; set; }
        public string CUSSP { get; set; }
        public string SCTRHealth { get; set; }
        public string SCTRPension { get; set; }
        public string PaymentType { get; set; }
        public bool? HasEPS { get; set; }
        public string EPSName { get; set; }

        public Bank Bank { get; set; }
        public Guid? BankId { get; set; }
        public string AccountNumber { get; set; }
        public byte? AccountType { get; set; }

        public Bank CTSBank { get; set; }
        public Guid? CTSBankId { get; set; }
        public string CTSAccountNumber { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<WorkerHistory> WorkerHistories { get; set; }
    }
}
