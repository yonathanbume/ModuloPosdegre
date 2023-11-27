using AKDEMIC.CORE.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ExtracurricularCourseViewModels
{
    public class ExtracurricularCourseViewModel
    {
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [StringLength(10, ErrorMessage = "El campo no puede exceder {0} caracteres.")]
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [StringLength(100, ErrorMessage = "El campo no puede exceder {0} caracteres.")]
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [StringLength(400, ErrorMessage = "El campo no puede exceder {0} caracteres.")]
        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Precio", Prompt = "Precio")]
        [Range(0.0, double.MaxValue, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.MINIMUM_VALUE)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Créditos", Prompt = "Créditos")]
        [Range(0.0, double.MaxValue, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.MINIMUM_VALUE)]
        public decimal Credits { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Área Extracurricular", Prompt = "Área Extracurricular")]
        public Guid AreaId { get; set; }
    }
}
