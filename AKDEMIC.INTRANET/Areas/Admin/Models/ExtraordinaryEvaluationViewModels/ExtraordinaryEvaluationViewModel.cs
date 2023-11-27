using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ExtraordinaryEvaluationViewModels
{
    public class ExtraordinaryEvaluationViewModel
    {
        public Guid? Id { get; set; }
        public Guid CourseId { get; set; }
        public string TeacherId { get; set; }
        public List<string> Committee { get; set; }
        public string Resolution { get; set; }
        public IFormFile ResolutionFile { get; set; }
        public string ResolutionFileUrl { get; set; }
        public string Course { get; set; }
        public string Teacher { get; set; }
        public string TermName { get; set; }
        public byte Type { get; set; }
    }
}
