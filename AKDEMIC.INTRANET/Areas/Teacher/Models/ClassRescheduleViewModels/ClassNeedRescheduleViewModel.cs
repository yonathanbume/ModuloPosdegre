using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.ClassRescheduleViewModels
{
    public class ClassNeedRescheduleViewModel
    {
        public Guid ClassId { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Justification { get; set; }
    }
}
