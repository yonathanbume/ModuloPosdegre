using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class ConferenceUser
    {
        public Guid Id { get; set; }
        public Guid ConferenceId { get; set; }
        public Conference Conference { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
