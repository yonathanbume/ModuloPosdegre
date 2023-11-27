using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class BalanceTransfer : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public Guid FromDependencyId { get; set; }

        public Guid ToDependencyId { get; set; }

        public Dependency FromDependency { get; set; }

        public Dependency ToDependency { get; set; }

        public bool IsCutTransfer { get; set; }

        public byte CutType { get; set; } = 1; // 1 - salida | 2 - ingreso

        public string ReferenceDocument { get; set; }

        public string Order { get; set; }

        public string Concept { get; set; }

        public DateTime Date { get; set; }
    }
}
