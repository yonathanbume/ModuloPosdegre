using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.TeachingManagement
{
    public class PerformanceEvaluationRatingScale
    {
        public Guid Id { get; set; }
        public byte MaxScore { get; set; }
        public string Description { get; set; }
        public byte Value { get; set; }
    }
}
