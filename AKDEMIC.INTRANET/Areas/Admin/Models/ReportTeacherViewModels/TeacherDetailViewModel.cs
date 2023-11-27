using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ReportTeacherViewModels
{
    public class TeacherDetailViewModel
    {
        public string TeacherId { get; set; }
        public string Teacher { get; set; }
        public string Username { get; set; }
        public string AcademicDepartment { get; set; }
        public Guid TermId { get; set; }
    }
}
