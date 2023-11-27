using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class JobOfferLanguage
    {
        public Guid LanguageId { get; set; }

        public Guid JobOfferId { get; set; }

        public byte Level { get; set; } = 1;

        public JobOffer JobOffer { get; set; }

        public Language Language { get; set; }
    }
}
