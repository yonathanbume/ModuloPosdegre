using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.INTRANET.Helpers;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SectionsViewModels
{
    public class SectionViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Id")]
        public Guid SectionId { get; set; }
        public string TeacherId { get; set; }

        public SectionFieldsViewModel Fields { get; set; }

        public ClassScheduleFieldsViewModel ClassSchedule { get; set; }
    }
}
