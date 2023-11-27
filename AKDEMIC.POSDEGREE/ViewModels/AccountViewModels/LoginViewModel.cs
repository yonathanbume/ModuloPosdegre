using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.POSDEGREE.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        /*[Required]
        [EmailAddress]
        public string Email { get; set; }*/

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
