using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.ClassRescheduleViewModels
{
    public class ClassRescheduleViewModel
    {
        public Guid Id { get; set; }

        public Guid ClassId { get; set; }
        public string UserId { get; set; }

        public string EndTime { get; set; }
        public string Justification { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public int Status { get; set; } = 1;
        public bool IsPermanent { get; set; }

        public Dictionary<string, int> ClassRescheduleStatusIndices { get; set; }
        public Dictionary<string, string> ClassRescheduleStatusValues { get; set; }
    }
}
