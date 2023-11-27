using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.INTRANET.Helpers;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SectionsViewModels
{
    public class ClassScheduleViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Id")]
        public Guid ClassScheduleId { get; set; }

        public ClassScheduleFieldsViewModel Fields { get; set; }
    }
}
