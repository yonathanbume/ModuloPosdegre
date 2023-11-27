using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string UserName { get; set; }
    }
}
