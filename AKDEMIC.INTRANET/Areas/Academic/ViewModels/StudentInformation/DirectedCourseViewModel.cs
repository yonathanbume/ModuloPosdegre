using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class DirectedCourseViewModel
    {
        public Guid StudentId { get; set; }

        [Required]
        public Guid CourseId { get; set; }

        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string Resolution { get; set; }

        [Required]
        public string TeacherId { get; set; }
    }
}
