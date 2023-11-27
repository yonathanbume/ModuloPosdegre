using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class Conference : Entity,ISoftDelete, ITimestamp
    {
        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public byte Type { get; set; } = ConstantHelpers.INTEREST_GROUP_CONFERENCE.TYPE.HANGOUT;
        public decimal Duration { get; set; }
        public string Title { get; set; }
        public Guid InterestGroupId { get; set; }
        public InterestGroup InterestGroup { get; set; }
        public string HangoutCreatorEmail { get; set; }
        public string HangoutLink { get; set; }
        public string GoogleEventId { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User{ get; set; }

        public ICollection<ConferenceUser> ConferenceUsers { get; set; }

        [NotMapped]
        public string StartFormattedDate { get; set; }
        [NotMapped]
        public string StartFormattedDateTime { get; set; }
        [NotMapped]
        public string InterestGroupName { get; set; }
        [NotMapped]
        public bool Finished => DateTime.UtcNow > EndDateTime ? true : false;
        [NotMapped]
        public bool CanEnter => DateTime.UtcNow >= StartDateTime ? true : false;
        [NotMapped]
        public string TypeString { get; set; }

    }
}
