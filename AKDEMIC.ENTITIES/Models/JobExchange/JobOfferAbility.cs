using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class JobOfferAbility
    {
        public Guid JobOfferId { get; set; }

        public Guid AbilityId { get; set; }

        public byte Level { get; set; }

        public JobOffer JobOffer { get; set; }

        public Ability Ability { get; set; }
    }
}
