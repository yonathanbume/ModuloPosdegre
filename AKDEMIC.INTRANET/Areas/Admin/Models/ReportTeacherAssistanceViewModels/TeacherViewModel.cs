using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ReportTeacherAssistanceViewModels
{
    public class TeacherViewModel
    {
        public string TeacherId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Career { get; set; }
        public string Faculty { get; set; }
        public string Image { get; set; }
        public string AcademicDepartment { get; set; }
        public string Term { get; set; }
        public List<ReportSectionAssistanceViewModel> Sections { get; set; }
    }
}
