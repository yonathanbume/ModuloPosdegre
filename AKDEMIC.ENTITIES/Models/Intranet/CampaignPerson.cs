using System;
using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class CampaignPerson : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid CampaignId { get; set; }
        public Guid? StudentId { get; set; }
        public Guid? ExternalPersonId { get; set; }
        public string Commentary { get; set; }

        public Campaign Campaign { get; set; }
        public Student Student { get; set; }
        public ExternalPerson ExternalPerson { get; set; }
    }
}
