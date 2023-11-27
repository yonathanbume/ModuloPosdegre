using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.UserRequirement
{
    public class UserRequirementOrderDetailTemplate
    {
        public Guid Id { get; set; }
        public Guid RequirementId { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Title { get; set; }
        public string Need { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        
        public string SupplierRUC { get; set; }
        public string SupplierName { get; set; }

        public string Dependency { get; set; }
        public decimal Cost { get; set; }

        public bool IsDelivered { get; set; }
        public string IsDeliveredText { get; set; }

        public bool Type { get; set; }
        public string TypeText { get; set; }

        public int Status { get; set; }
        public string StatusText { get; set; }

        public int FundingSource{ get; set; }
        public string FundingSourceText { get; set; }
    }
}
