using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class UserResearchArea
    {
        public Guid Id { get; set; }
        public ResearchArea ResearchArea { get; set; }
        public Guid ResearchAreaId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
