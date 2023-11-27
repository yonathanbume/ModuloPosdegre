using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class FinancialExecution : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte Type { get; set; }

        public ICollection<FinancialExecutionDetail> Details { get; set; }
    }
}
