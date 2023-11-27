using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class JobOfferCareer
    {
        public Guid CareerId { get; set; }

        public Guid JobOfferId { get; set; }

        public Career Career { get; set; }

        public JobOffer JobOffer { get; set; }
    }
}
