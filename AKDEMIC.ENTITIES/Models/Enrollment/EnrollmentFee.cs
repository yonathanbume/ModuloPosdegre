using AKDEMIC.ENTITIES.Base.Implementations;
using AKDEMIC.ENTITIES.Base.Interfaces;
using AKDEMIC.ENTITIES.Models.Admission;
using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EnrollmentFee : Entity, ITimestamp
    {
        public Guid Id { get; set; }

        public Guid EnrollmentFeeTermId { get; set; }

        public int Count { get; set; }

        public decimal Total { get; set; }

        public EnrollmentFeeTerm EnrollmentFeeTerm { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<EnrollmentFeeDetail> Details { get; set; }
    }
}
