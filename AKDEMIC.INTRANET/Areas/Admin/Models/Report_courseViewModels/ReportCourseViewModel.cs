using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.Report_courseViewModels
{
    public class ReportCourseViewModel
    {
        public Guid IdCourseTerm { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Credits { get; set; }
        public string Section { get; set; }
        public Guid Id { get; set; }
        public int TotalClasses { get; set; }
        public string Teachers { get; set; }
    }
}
