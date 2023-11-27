using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class Campaign
    {
        public Guid Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string Topic { get; set; }
        public string Place { get; set; }
        public DateTime DateInitTime { get; set; }
        public DateTime DateFinishTime { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<CampaignPerson> CampaignPersons { get; set; }
    }
}
