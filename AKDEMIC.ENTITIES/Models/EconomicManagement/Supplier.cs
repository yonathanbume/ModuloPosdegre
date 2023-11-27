using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.EconomicManagement
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid SupplierCategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string RUC { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int State { get; set; }

        [NotMapped]
        public string StateText { get; set; }

        public ApplicationUser User { get; set; }
        public SupplierCategory SupplierCategory { get; set; }
        public ICollection<RequirementSupplier> RequirementSuppliers { get; set; }
    }
}
