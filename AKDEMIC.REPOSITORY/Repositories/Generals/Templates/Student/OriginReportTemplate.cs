using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Generals.Templates.Student
{
    public class OriginReportTemplate
    {
        public string TermName { get; set; }
        public string Career { get; set; }
        public string Department { get; set; }
        public string District { get; set; }
        public string Province { get; set; }

        public string OfficeName { get; set; }

        public int Male { get; set; }
        public int Fermale { get; set; }
        public int None { get; set; }
        public int Total { get; set; }
    }


}
