using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class AcademicYearCredit
    {
        public Guid CurriculumId { get; set; }

        public byte AcademicYear { get; set; }

        public decimal Credits { get; set; }

        public decimal StartCredits { get; set; }

        public decimal EndCredits { get; set; }

        public Curriculum Curriculum { get; set; }
    }
}
