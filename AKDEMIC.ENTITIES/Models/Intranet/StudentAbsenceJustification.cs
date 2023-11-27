using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Generals;
using System;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class StudentAbsenceJustification
    {
        public Guid Id { get; set; }
        
        public ClassStudent ClassStudent { get; set; }
        public Guid ClassStudentId { get; set; }

        public DateTime RegisterDate { get; set; }

        public string Justification { get; set; }

        public string Observation { get; set; }

        public string File { get; set; }

        public string TeacherId { get; set; }

        public Teacher Teacher { get; set; }

        public int Status { get; set; } = ConstantHelpers.Intranet.StudentAbsenceJustification.Status.REQUESTED;
    }
}
