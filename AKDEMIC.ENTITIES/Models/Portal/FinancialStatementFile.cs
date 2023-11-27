using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;

namespace AKDEMIC.ENTITIES.Models.Portal
{
    public class FinancialStatementFile : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
        public bool IsLink { get; set; }
        public Guid FinancialStatementId { get; set; }

        public FinancialStatement FinancialStatement { get; set; }
    }
}
