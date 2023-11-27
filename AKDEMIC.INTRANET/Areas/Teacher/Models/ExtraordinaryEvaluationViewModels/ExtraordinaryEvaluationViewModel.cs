using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.ExtraordinaryEvaluationViewModels
{
    public class ExtraordinaryEvaluationViewModel
    {
        public Guid Id { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Term { get; set; }
        public string Teacher { get; set; }
        public List<string> Committee { get; set; }
        public string ResolutionFileUrl { get; set; }
    }
}
