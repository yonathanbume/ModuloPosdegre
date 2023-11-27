using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EnrollmentFeeDetailTemplate
    {
        public Guid Id { get; set; }

        public Guid EnrollmentFeeTemplateId { get; set; }

        public int FeeNumber { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime DueDate { get; set; }

        public EnrollmentFeeTemplate EnrollmentFeeTemplate { get; set; }
    }
}
