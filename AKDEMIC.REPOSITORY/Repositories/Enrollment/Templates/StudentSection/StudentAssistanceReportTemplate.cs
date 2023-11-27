using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.Enrollment.Templates.StudentSection
{
    public class StudentAssistanceReportTemplate
    {
        public string student { get;  set; }
        public string StudentUsername { get; set; }
        public int absences { get;  set; }
        public int assisted { get;  set; }
        public int dictated { get;  set; }
        public int maxAbsences { get;  set; }
    }
}
