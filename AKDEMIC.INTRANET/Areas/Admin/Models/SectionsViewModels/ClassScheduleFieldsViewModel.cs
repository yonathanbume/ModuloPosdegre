using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.INTRANET.Helpers;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SectionsViewModels
{
    public class ClassScheduleFieldsViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Empieza")]
        [DataType(DataType.Time)]
        public string StartTime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Termina")]
        [DataType(DataType.Time)]
        public string EndTime { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Range(0, 6, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.RANGE)]
        [Display(Name = "Día")]
        public int WeekDay { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Range(1, 2, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.RANGE)]
        [Display(Name = "Tipo de Sesión")]
        public int SessionType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Sección")]
        public Guid SectionId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Profesores")]
        public List<string> SelectedTeachers { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Aula")]
        public Guid ClassroomId { get; set; }
    }
}
