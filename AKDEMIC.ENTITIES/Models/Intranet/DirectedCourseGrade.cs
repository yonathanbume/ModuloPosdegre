using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Intranet
{
    public class DirectedCourseGrade
    {
        public Guid StudentSectionId { get; set; }

        public byte EvaluationNumber { get; set; }

        public decimal Grade { get; set; }
    }
}
