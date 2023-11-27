using System;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class CatalogGoal
    {
        public Guid Id { get; set; }
        public string SecFunc { get; set; }
        public string Program { get; set; }
        public string ProdPry { get; set; }
        public string ActWork { get; set; }
        public string Function { get; set; }
        public string DivisionFunc { get; set; }
        public string Description { get; set; }
        public DateTime CreateAt { get; set; }
        public string Year { get; set; }

        public CatalogGoal()
        {
            CreateAt = DateTime.UtcNow;
        }
    }
}
