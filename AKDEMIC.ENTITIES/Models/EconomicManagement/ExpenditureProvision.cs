using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class ExpenditureProvision : Entity, ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string ReferenceDocument { get; set; }

        public string Order { get; set; }

        public string Concept { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public byte Status { get; set; }

        public Guid DependencyId { get; set; }

        public Dependency Dependency { get; set; }
    }
}
