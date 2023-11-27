using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.CORE.Overrides;
using AKDEMIC.INTRANET.Helpers;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.Areas.Student.Models.AbsenceJustificationViewModels
{
    public class AbsenceJustificationViewModel
    {
        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Clase")]
        public Guid ClassStudentId { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [StringLength(500, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.STRING_LENGTH)]
        [Display(Name = "Justificación")]
        public string Justification { get; set; }

        [DataType(DataType.Upload)]
        [Extensions("jpg,jpeg,png,bmp,svg,gif,pdf", ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.FILE_EXTENSIONS)]
        [Display(Name = "Archivo de apoyo (opcional)")]
        public IFormFile File { get; set; }

        [StringLength(500, ErrorMessage = CORE.Helpers.ConstantHelpers.MESSAGES.VALIDATION.STRING_LENGTH)]
        [Display(Name = "Observación")]
        public string Observation { get; set; }
    }
}
