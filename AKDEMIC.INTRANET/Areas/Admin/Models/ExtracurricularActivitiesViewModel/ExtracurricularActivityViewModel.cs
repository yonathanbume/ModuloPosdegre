using AKDEMIC.CORE.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.ExtracurricularActivitiesViewModel
{
    public class ExtracurricularActivityViewModel
    {
        public Guid? Id { get; set; }

        [StringLength(maximumLength: 10, MinimumLength = 1, ErrorMessage = "El campo '{0}' debe tener como máximo {1} letras")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "El campo '{0}' debe tener como máximo {1} letras")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [StringLength(maximumLength: 400, MinimumLength = 0, ErrorMessage = "El campo '{0}' debe tener como máximo {1} letras")]
        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Créditos", Prompt = "Créditos")]
        [Range(0.0, double.MaxValue, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.MINIMUM_VALUE)]
        public decimal Credits { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Área Extracurricular", Prompt = "Área Extracurricular")]
        public Guid AreaId { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Periodo Académico", Prompt = "Periodo Académico")]
        public Guid TermId { get; set; }
    }
}
