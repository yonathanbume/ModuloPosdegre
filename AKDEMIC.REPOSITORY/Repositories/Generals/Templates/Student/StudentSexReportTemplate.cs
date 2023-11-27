using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class StudentSexReportTemplate
    {
        public string Term { get; set; }
        public string Logo { get; set; }
        public string SuperiorText { get; set; }
        public string HeaderText { get; set; }
        public string SubheaderText { get; set; }
        public List<StudentSexReportDetailTemplate> Details { get; set; }
    }

    public class StudentSexReportDetailTemplate
    {
        public string Career { get; set; }
        public string CareerCode { get; set; }
        public int Male { get; set; }
        public int Fermale { get; set; }
        public int None { get; set; }
        public int Total => Fermale + Male + None;
    }
}
