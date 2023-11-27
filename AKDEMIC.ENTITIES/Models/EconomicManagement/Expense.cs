using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class Expense
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Invoice { get; set; }

        public string ReferenceDocument { get; set; }

        public string Order { get; set; }

        public string Concept { get; set; }

        public DateTime Date { get; set; }

        public DateTime Month { get; set; }

        public decimal Amount { get; set; }

        public byte Type { get; set; } = 2; //0 - saldo anterior / 1 - devolucion / 2 - gasto / 3 - anulacion

        public bool IsCanceled { get; set; }

        public string Comment { get; set; }

        public Guid DependencyId { get; set; }

        public Dependency Dependency { get; set; }

        public Guid? RelatedOrderId { get; set; }

        public Order RelatedOrder { get; set; }
    }
}
