using System;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.GradeViewModels
{
    public class GradeViewModel
    {
        public int EvaluationId { get; set; }
        public string Evaluation { get; set; }
        public decimal Value { get; set; }
    }

    public class GradePostViewModel
    {
        public Guid Id { get; set; }
        public bool NotTaken { get; set; }
        public decimal Grade { get; set; }
    }
}
