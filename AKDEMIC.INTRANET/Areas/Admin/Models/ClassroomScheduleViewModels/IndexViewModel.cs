using System;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ClassroomScheduleViewModels
{
    public class IndexViewModel
    {
        public TermViewModel Term { get; set; }
    }

    public class TermViewModel
    {
        public string Name { get; set; }

        public DateTime ClassStartDate { get; set; }

        public DateTime ClassEndDate { get; set; }
    }
}