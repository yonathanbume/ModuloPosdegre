using System;

namespace AKDEMIC.INTRANET.Areas.CareerDirector.Models.Report_courseViewModels
{
    public class ReportCourseViewModel
    {
        public Guid IdCourseTerm { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Credits { get; set; }

    }
}
