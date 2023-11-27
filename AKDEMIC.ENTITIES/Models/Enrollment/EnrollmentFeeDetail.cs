using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EnrollmentFeeDetail : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid EnrollmentFeeId { get; set; }
        public Guid TermId { get; set; }

        public int FeeNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public decimal Amount { get; set; }

        public bool WasGenerated { get; set; }

        public EnrollmentFee EnrollmentFee { get; set; }
        public Term Term { get; set; }
    }
}
