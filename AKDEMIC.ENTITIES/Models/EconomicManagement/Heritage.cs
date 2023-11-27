using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.DocumentaryProcedure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class Heritage : Entity, ITimestamp
    {
        [Key]
        public Guid DependencyId { get; set; }
        [Key]
        public Guid CatalogItemId { get; set; }
        public int Quantity { get; set; }
        public string Code { get; set; }
        public string Ubication { get; set; }
        public string PecosaCode { get; set; }
        public CatalogItem CatalogItem { get; set; }
        public Dependency Dependency { get; set; }
    }
}
