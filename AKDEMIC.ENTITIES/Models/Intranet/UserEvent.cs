using System;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class UserEvent
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid EventId { get; set; }
        public Guid? ExternalUserId { get; set; }

        public ApplicationUser User { get; set; }
        public Event Event { get; set; }
        public ExternalUser ExternalUser { get; set; }

        public bool Absent { get; set; }
    }
}
