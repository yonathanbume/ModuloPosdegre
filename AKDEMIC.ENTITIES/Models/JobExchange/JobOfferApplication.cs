using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.JobExchange
{
    public class JobOfferApplication
    {
        public Guid JobOfferId { get; set; }

        public Guid StudentId { get; set; }

        public byte Status { get; set; } = 1;

        public JobOffer JobOffer { get; set; }

        public Student Student { get; set; }

        public DateTime Date { get; set; }
    }
}
