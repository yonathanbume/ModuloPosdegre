using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class SupplierCategory
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        public ICollection<Supplier> Suppliers { get; set; }

    }
}
