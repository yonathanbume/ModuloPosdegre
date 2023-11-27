using AKDEMIC.CORE.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ExtracurricularCourseGroupViewModels
{
    public class ExtracurricularCourseGroupViewModel
    {
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [StringLength(10, ErrorMessage = "El campo no puede exceder {0} caracteres.")]
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Docente", Prompt = "Docente")]
        public string TeacherId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Curso Extracurricular", Prompt = "Curso Extracurricular")]
        public Guid ExtracurricularCourseId { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Periodo Académico", Prompt = "Periodo Académico")]
        public Guid TermId { get; set; }
    }
}
