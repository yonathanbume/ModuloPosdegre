using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Academic.ViewModels.StudentInformation
{
    public class ChangePasswordViewModel
    {
        public StudentViewModel Student { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "La {0} debe tener por lo menos {2} caracteres y máximo {1}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = " Nueva contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña no coincide.")]
        public string ConfirmPassword { get; set; }
    }
}
