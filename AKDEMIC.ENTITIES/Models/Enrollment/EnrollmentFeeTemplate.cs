using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EnrollmentFeeTemplate
    {
        public Guid Id { get; set; }
        public int Count { get; set; }

        public ICollection<EnrollmentFeeDetailTemplate> Details { get; set; }
    }
}
