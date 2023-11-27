using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class MeetingUser
    {
        public Guid Id { get; set; }

        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public bool Assistance { get; set; }

        [NotMapped]
        public string UserFullName { get; set; }
        [NotMapped]
        public string UserEmail { get; set; }
    }
}
