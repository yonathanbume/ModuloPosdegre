namespace AKDEMIC.INTRANET.Areas.CareerDirector.Models.StudentInformation
{
    public class CourseEvaluationViewModel
    {
        public string Name { get; set; }

        public int Percentage { get; set; }

        public bool Attended { get; set; }

        public bool Approved { get; set; }

        public decimal Grade { get; set; }

        public bool Taked { get; set; } = false;
    }
}
