using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.DirectedCourseViewModels
{
    public class GradeViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Nota final obtenida")]
        public decimal Grade { get; set; }
    }
}
