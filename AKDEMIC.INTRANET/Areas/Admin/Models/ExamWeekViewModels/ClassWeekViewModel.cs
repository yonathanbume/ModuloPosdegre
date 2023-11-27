using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ExamWeekViewModels
{
    public class ClassWeekViewModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Week { get; set; }
    }
}
