using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class InterestGroupUser
    {
        public Guid InterestGroupId { get; set; }
        public InterestGroup InterestGroup { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [NotMapped]
        public string UserFullName { get; set; }
        [NotMapped]
        public string UserEmail { get; set; }
    }
}
