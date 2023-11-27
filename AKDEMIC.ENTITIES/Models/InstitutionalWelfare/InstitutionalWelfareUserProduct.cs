using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class InstitutionalWelfareUserProduct
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid InstitutionalWelfareProductId { get; set; }
        public ApplicationUser User { get; set; }
        public InstitutionalWelfareProduct InstitutionalWelfareProduct { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int Quantity { get; set; }
        public bool WasReturned { get; set; }
        public string Commentary { get; set; }
    }
}
