using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.CORE.Overrides;
using AKDEMIC.INTRANET.Helpers;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.ViewModels.AbsenceJustificationViewModels
{
    public class AbsenceJustificationViewModel
    {
        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Día de Trabajo Inasistido")]
        public Guid WorkingDayId { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [StringLength(200, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.STRING_LENGTH)]
        [Display(Name = "Justificación")]
        public string Justification { get; set; }

        [DataType(DataType.Upload)]
        [Extensions("jpg,jpeg,png,bmp,svg,gif,pdf", ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.FILE_EXTENSIONS)]
        [Display(Name = "Archivo de apoyo (opcional)")]
        public IFormFile File { get; set; }
    }
}
