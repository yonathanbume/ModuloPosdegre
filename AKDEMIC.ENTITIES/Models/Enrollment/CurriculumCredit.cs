using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CurriculumCredit
    {
        public Guid Id { get; set; }

        public Guid CurriculumId { get; set; }

        public decimal MaxCredits { get; set; }

        public decimal MinCredits { get; set; }

        public decimal CreditsP1 { get; set; }

        public decimal CreditsP2 { get; set; }

        public decimal CreditsObservation { get; set; }

        public decimal CreditsDisapproved { get; set; }

        public decimal CreditsPro { get; set; }

        public decimal Average2sc { get; set; }

        public decimal AverageMn1 { get; set; }

        public decimal AverageMn2 { get; set; }

        public decimal AverageMx1 { get; set; }

        public decimal AverageMx2 { get; set; }

        public Curriculum Curriculum { get; set; }
    }
}
