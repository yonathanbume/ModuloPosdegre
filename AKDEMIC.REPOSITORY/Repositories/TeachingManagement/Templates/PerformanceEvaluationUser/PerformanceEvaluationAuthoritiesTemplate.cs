using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluationUser
{
    public class PerformanceEvaluationAuthoritiesTemplate
    {
        public string Faculty { get; set; }
        public string Position { get; set; }
        public string FullName { get; set; }
        public int Programmed { get; set; }
        public int Evaluated { get; set; }
        public bool IsCompleted => Programmed == Evaluated;
        public string Status => Programmed != 0 ? $"{((decimal)Evaluated / (decimal)Programmed * 100):F}" : "0.00";
    }
}
