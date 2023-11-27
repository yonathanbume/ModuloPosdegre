using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class EnrollmentShift
    {
        public Guid Id { get; set; }
        public Guid? ResolutionId { get; set; }
        public Guid TermId { get; set; }

        public bool WasExecuted { get; set; } = false;

        public Resolution Resolution { get; set; }
        public Term Term { get; set; }
        public ICollection<CareerEnrollmentShift> CareerEnrollmentShifts { get; set; }
    }
}
