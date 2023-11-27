using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.ViewModels.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
