using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class StructureForExpense
    {
        public Guid Id { get; set; }
        public Guid DependencyId { get; set; }
        public int ExecutionYear { get; set; }
        public string ExecutionMonth { get; set; }
        public int CaseFile { get; set; }
        public string FinancingSource { get; set; }
        public int DocumentCod { get; set; }
        public string DocumentName { get; set; }
        public int DocumentNum { get; set; }
        public DateTime DocumentDate { get; set; }
        public int ExpenseClassifier { get; set; }
        public string ExpenseClassifierName { get; set; }
        public int ProjectActivityCode { get; set; }
        public string ProjectActivityName { get; set; }
        public string Goals { get; set; }
        public decimal Amount { get; set; }
        public string Provider { get; set; }
        public int POAOperationalActivityCod { get; set; }
        public string POAOperationalActivityName { get; set; }

        public DateTime CreateAt { get; set; }
        public Dependency Dependency { get; set; }
        public StructureForExpense()
        {
            CreateAt = DateTime.UtcNow;
        }
    }
}
