using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.TeachingManagement.Templates.ScoreInputSchedule
{
    public class ScoreInputScheduleViewModel
    {
        public string AcademicDepartment { get; set; }
        public string Career { get; set; }
        public string UnitComponents { get; set; }
        public List<ScoreInputScheduleDetailViewModel> Details { get; set; }
    }
}
