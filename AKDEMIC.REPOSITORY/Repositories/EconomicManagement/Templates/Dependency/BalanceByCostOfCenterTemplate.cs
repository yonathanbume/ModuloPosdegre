using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.EconomicManagement.Templates.Dependency
{
    public class BalanceByCostOfCenterTemplate
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Dependency { get; set; }
        public decimal Balance { get; set; }
        public decimal Available { get; set; }
    }
}
