using AKDEMIC.INTRANET.Helpers;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.DependencyViewModels
{
    public class DependencyFieldsViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
}
