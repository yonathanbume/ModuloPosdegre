using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation.PdfViewModels
{
    public class AcademicHistoryCoursePdfViewModel
    {
        public string ImagePathLogo { get; set; }
        public string JsPath { get; set; }
        public DateTime Today { get; set; }
        public StudentInfoViewModel StudentInfo { get; set; }
        public HistoryCourseViewModel[] Details { get; set; }
        public AcademicHistoryCoursePdfViewModel()
        {
            Details = new HistoryCourseViewModel[] { };
            StudentInfo = new StudentInfoViewModel();
        }
    }
    public class HistoryCourseViewModel
    {
        public string Year { get; set; }
        public string Code { get; set; }
        public string Course { get; set; }
        public string Credits { get; set; }
        public int Grade { get; set; }
        public bool Validated { get; set; }
        public string Term { get; set; }
        public string Status { get; set; }
        public string Observations { get; set; }
        public bool Approved { get; set; }
        public byte Type { get; set; }
        public bool Withdraw { get; set; }
    }
}
