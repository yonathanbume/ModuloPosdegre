using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class StructureForIncome
    {
        public Guid Id { get; set; }
        public Guid DependencyId { get; set; }
        public int ExecutionYear { get; set; }
        public string ExecutionMonth { get; set; }
        public int CaseFile { get; set; }
        public string FinancingSource { get; set; }
        public int DocumentCod { get; set; }
        public string DocumentName { get; set; }
        public string DocumentNum { get; set; }
        public DateTime DocumentDate { get; set; }
        public decimal Amount { get; set; }
        public int IncomeClassifier { get; set; }
        public string IncomeClassifierName { get; set; }
        public DateTime CreateAt { get; set; }
        public Dependency Dependency { get; set; }

        public StructureForIncome()
        {
            CreateAt = DateTime.UtcNow;
        }
    }
}
