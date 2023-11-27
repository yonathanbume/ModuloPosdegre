using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.ViewModels.APIQuibukViewModels
{
    public class ForgotPasswordQuibukViewnModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
