using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.INTRANET.Helpers;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SectionsViewModels
{
    public class SectionFieldsViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "CursoPeriodo")]
        public Guid CourseTermId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Profesor")]
        public string TeacherId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Range(1, int.MaxValue, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.RANGE)]
        [Display(Name = "Vacantes")]        
        public int Vacancies { get; set; }
    }
}
