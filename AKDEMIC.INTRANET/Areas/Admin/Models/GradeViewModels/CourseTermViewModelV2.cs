using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.GradeViewModels
{
    public class CourseTermViewModelV2
    {
        public Guid CourseTermId { get; set; }
        public string Course { get; set; }
        public string Term { get; set; }
        public string Curriculum { get; set; }
        public string Career { get; set; }
    }
}
