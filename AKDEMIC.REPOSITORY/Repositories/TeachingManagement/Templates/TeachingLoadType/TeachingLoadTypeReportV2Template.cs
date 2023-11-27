using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.TeachingLoadType
{
    public class TeachingLoadTypeReportV2Template
    {
        public string TeacherUserName { get; set; }
        public string Teacher { get; set; }
        public string TeachingLoadType { get; set; }
        public string Name { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Hours { get; set; }

        public string Monday { get; set; }
        public string Tuesday { get; set; }
        public string Wednesday { get; set; }
        public string Thursday { get; set; }
        public string Friday { get; set; }
        public string Saturday { get; set; }
    }
}
