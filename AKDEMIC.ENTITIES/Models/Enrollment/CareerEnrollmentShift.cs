using System;
using AKDEMIC.ENTITIES.Models.Generals;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class CareerEnrollmentShift
    {
        public Guid Id { get; set; }

        public Guid CareerId { get; set; }

        public bool WasExecuted { get; set; } = false;

        public DateTime? LastExecution { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public Guid EnrollmentShiftId { get; set; }

        public EnrollmentShift EnrollmentShift { get; set; }

        public Career Career { get; set; }
    }
}
