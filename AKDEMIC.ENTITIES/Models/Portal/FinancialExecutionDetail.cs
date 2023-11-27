using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class FinancialExecutionDetail : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FilePath { get; set; }

        public bool IsLink { get; set; }
        public Guid FinancialExecutionId { get; set; }

        public FinancialExecution FinancialExecution { get; set; }
    }
}
