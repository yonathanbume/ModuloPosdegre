using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.CORE.Overrides;
using AKDEMIC.INTRANET.Helpers;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.StudentViewModels
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Nombres", Prompt = "Nombres")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Apellido Paterno", Prompt = "Apellido Paterno")]
        public string PaternalSurname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Apellido Materno", Prompt = "Apellido Materno")]
        public string MaternalSurname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "{0} no válido")]
        [Display(Name = "Correo electrónico", Prompt = "Correo Electrónico")]
        public string Email { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [StringLength(maximumLength: 9, MinimumLength = 5, ErrorMessage = "El campo '{0}' debe tener {1} dígitos")]
        [RegularExpression("[1-9][0-9]*", ErrorMessage = "{0} no válido")]
        [Display(Name = "Teléfono", Prompt = "Teléfono")]
        public string PhoneNumber { get; set; }

        [StringLength(maximumLength: 9, MinimumLength = 5, ErrorMessage = "El campo '{0}' debe tener {1} dígitos")]
        [RegularExpression("[1-9][0-9]*", ErrorMessage = "{0} no válido")]
        [Display(Name = "Teléfono 2", Prompt = "Teléfono 2")]
        public string PhoneNumber2 { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Usuario", Prompt = "Usuario")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Fecha de Nacimiento", Prompt = "Fecha de Nacimiento")]
        public string BirthDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [StringLength(maximumLength: 8, MinimumLength = 8, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.STRING_LENGTH)]
        [RegularExpression("[0-9]*", ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REGULAR_EXPRESSION)]
        [Display(Name = "DNI", Prompt = "DNI")]
        public string Dni { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Range(1, 2, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.RANGE)]
        [Display(Name = "Sexo", Prompt = "Sexo")]
        public int Sex { get; set; }

        [Display(Name = "Dirección", Prompt = "Dirección")]
        public string Address { get; set; }

        [DataType(DataType.Upload)]
        [Extensions(ConstantHelpers.DOCUMENTS.FILE_EXTENSION_GROUP.IMAGES, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.FILE_EXTENSIONS)]
        [Display(Name = "Foto", Prompt = "Foto")]
        public IFormFile Picture { get; set; }

        public string PictureUrl { get; set; }
        
        [DataType(DataType.Password)]
        [StringLength(maximumLength: 256, MinimumLength = 6, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.STRING_LENGTH)]
        [Display(Name = "Contraseña", Prompt = "Contraseña")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.COMPARE)]
        [Display(Name = "Confirmar contraseña", Prompt = "Confirmar Contraseña")]
        public string ConfirmedPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Facultad", Prompt = "Facultad")]
        public Guid FacultyId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Escuela", Prompt = "Escuela")]
        public Guid SelectedCareer { get; set; }
    }
}
