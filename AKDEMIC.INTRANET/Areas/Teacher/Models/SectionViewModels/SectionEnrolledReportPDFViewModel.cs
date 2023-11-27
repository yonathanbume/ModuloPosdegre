using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.SectionViewModels
{
    public class SectionEnrolledReportPDFViewModel
    {
        public string HeaderText { get; set; }
        public string Image { get; set; }
        public SectionViewModel SectionDetail { get; set; }
        public List<EnrolledStudentViewModel> EnrolledStudents { get; set; }
    }

    public class SectionViewModel
    {
        public string Term { get; set; }
        public List<string> Teachers { get; set; }
        public string Faculty { get; set; }
        public string Career { get; set; }
        public string Course { get; set; }
        public string Modality { get; set; }
        public string Section { get; set; }
        public string Credits { get; set; }
        public string Level { get; set; }
        public string Semester { get; set; }
        public string SectionGroupName { get; set; }
    }

    public class EnrolledStudentViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int Try { get; set; }
        public string Modality { get; set; }
    }
}
