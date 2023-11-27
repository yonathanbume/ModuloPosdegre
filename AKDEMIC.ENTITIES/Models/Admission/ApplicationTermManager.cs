using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Admission
{
    public class ApplicationTermManager
    {
        public Guid Id { get; set; }
        public Guid ApplicationTermId { get; set; }
        public ApplicationTerm ApplicationTerm { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Description { get; set; }
    }
}
