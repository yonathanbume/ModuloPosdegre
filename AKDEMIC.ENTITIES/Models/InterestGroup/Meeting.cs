using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.ENTITIES.Models.InterestGroup
{
    public class Meeting : Entity , ITimestamp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime AnnouncementDate { get; set; }
        public string Matter { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }

        public string UserOwnerId { get; set; }

        public Guid InterestGroupId { get; set; }
        public InterestGroup InterestGroup { get; set; }

        public ICollection<MeetingFile> MeetingFiles { get; set; }
        public ICollection<MeetingUser> MeetingUsers { get; set; }
        public ICollection<MeetingCriterion> MeetingCriterions { get; set; }

        [NotMapped]
        public string AnnouncementDateFormatted { get; set; }
    }
}
