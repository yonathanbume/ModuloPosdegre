using AKDEMIC.INTRANET.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Teacher.Models.TutorialViewModels
{
    public class TutorialViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Aula")]
        public Guid ClassroomId { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Sección")]
        public Guid SectionId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Empieza")]
        [DataType(DataType.Time)]
        public string Start { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Termina")]
        [DataType(DataType.Time)]
        public string End { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public string Date { get; set; }
    }
}
