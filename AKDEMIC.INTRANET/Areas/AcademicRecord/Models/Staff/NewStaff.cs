using AKDEMIC.INTRANET.Helpers;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.AcademicRecord.Models.Staff
{
    public class NewStaff
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Nombre", Prompt = "Ingrese nombre.")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Apellido Materno", Prompt = "Ingrese apellido materno.")]
        public string MaternalSurname { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Apellido Paterno", Prompt = "Ingrese apellido paterno.")]
        public string PaternalSurname { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Usuario", Prompt = "Ingrese usuario.")]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Contraseña", Prompt = "Ingrese contraseña.")]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Verificar Contraseña", Prompt = "Ingrese contraseña.")]
        public string PasswordVerifier { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Celular", Prompt = "Ingrese celular.")]
        public string PhoneNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Correo Electrónico", Prompt = "Ingrese correo electrónico.")]
        public string Email { get; set; }
    }
}
