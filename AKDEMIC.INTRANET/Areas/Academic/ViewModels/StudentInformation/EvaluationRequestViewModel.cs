using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class EvaluationRequestViewModel
    {
        public Guid id { get; set; }
        public Guid CourseId { get; set; }
        public string TeacherId { get; set; }
        public List<string> TeachersCommitee { get; set; }
        public IFormFile File { get; set; }
        public string Resolution { get; set; }
    }
}
