using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.PerformanceEvaluationUser
{
    public class PerformationEvaluationStudentTemplate
    {
        public Guid CareerId { get; set; }
        public string Code { get; set; }
        public string Career { get; set; }
        public int Programmed { get; set; }
        public int Evaluated { get; set; }
        public bool IsCompleted => Programmed == Evaluated;
        public string Status => Programmed != 0 ? $"{((decimal)Evaluated / (decimal)Programmed * 100):F}" : "0.00";
    }
}
