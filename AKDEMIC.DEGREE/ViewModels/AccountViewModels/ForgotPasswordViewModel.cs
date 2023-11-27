using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DEGREE.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string UserName { get; set; }
    }
}
