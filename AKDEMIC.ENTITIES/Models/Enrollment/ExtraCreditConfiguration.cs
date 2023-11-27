using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class ExtraCreditConfiguration
    {
        public Guid Id { get; set; }

        public int? MeritType { get; set; }

        public decimal? AverageGradeStart { get; set; }

        public decimal? AverageGradeEnd { get; set; }

        public decimal Credits { get; set; }
    }
}
