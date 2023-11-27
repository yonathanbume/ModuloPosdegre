using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.Enrollment
{
    public class AverageGradeCreditConfiguration
    {
        public Guid Id { get; set; }

        public decimal Credits { get; set; }

        public decimal AverageGradeStart { get; set; }

        public decimal AverageGradeEnd { get; set; }

        public string Name { get; set; }

        public bool GreaterThan { get; set; }

        public bool LessThan { get; set; }
    }
}
