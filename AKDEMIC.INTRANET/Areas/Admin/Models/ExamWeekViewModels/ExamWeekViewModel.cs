using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ExamWeekViewModels
{
    public class ExamWeekViewModel
    {
        public Guid TermId { get; set; }
        public byte Type { get; set; }
        public int Week { get; set; }
    }
}
