using AKDEMIC.ENTITIES.Models.EconomicManagement;
using AKDEMIC.ENTITIES.Models.Generals;
using System;
using System.Collections.Generic;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class ExtracurricularCourseGroupStudent
    {
        public Guid Id { get; set; }

        public Guid GroupId { get; set; }
        public ExtracurricularCourseGroup Group { get; set; }

        public decimal Score { get; set; }

        public bool Approved { get; set; }

        public Guid? PaymentId { get; set; }
        public Payment Payment { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public ICollection<ExtracurricularCourseGroupAssistanceStudent> GroupAssistanceStudents { get; set; }
    }
}
