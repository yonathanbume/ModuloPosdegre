using System;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Models.StudentInformation
{
    public class EvaluationRequestViewModel
    {
        public Guid CourseId { get; set; }

        public bool IsApproved { get; set; }

        public int Grade { get; set; }

        public string Observations { get; set; }
    }
}
