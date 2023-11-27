using System;
using AKDEMIC.ENTITIES.Models.Enrollment;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class UserAbsenceJustification
    {
        public Guid Id { get; set; }

        public WorkingDay WorkingDay { get; set; }
        public Guid WorkingDayId { get; set; }

        public DateTime RegisterDate { get; set; }

        public string Justification { get; set; }

        public string File { get; set; }

        public int Status { get; set; } = 0;
    }
}
