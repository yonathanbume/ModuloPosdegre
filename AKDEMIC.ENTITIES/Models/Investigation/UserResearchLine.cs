using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Investigation
{
    public class UserResearchLine
    {
        public Guid Id { get; set; }
        public Guid ResearchLineId { get; set; }
        public ResearchLine ResearchLine { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public byte Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }
    }
}
