
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.CORE.Overrides;
using AKDEMIC.INTRANET.Helpers;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.UserViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Nombres")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Apellido Paterno")]
        public string PaternalSurname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Apellido Materno")]
        public string MaternalSurname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "{0} no válido")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [StringLength(maximumLength: 9, MinimumLength = 5, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.STRING_LENGTH)]
        [RegularExpression("[1-9][0-9]*", ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REGULAR_EXPRESSION)]
        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Fecha de Nacimiento")]
        public string BirthDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [StringLength(maximumLength: 8, MinimumLength = 8, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.STRING_LENGTH)]
        [RegularExpression("[0-9]*", ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REGULAR_EXPRESSION)]
        [Display(Name = "DNI")]
        public string Dni { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Range(0, 2, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.RANGE)]
        [Display(Name = "Sexo")]
        public int Sex { get; set; } = 1;

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [DataType(DataType.Upload)]
        [Extensions(ConstantHelpers.DOCUMENTS.FILE_EXTENSION_GROUP.IMAGES, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.FILE_EXTENSIONS)]
        [Display(Name = "Foto")]
        public IFormFile Picture { get; set; }

        public string PictureUrl { get; set; }

        [Display(Name = "Activo")]
        public bool IsActive { get; set; }

        [DataType(DataType.Password)]
        [StringLength(maximumLength: 256, MinimumLength = 6, ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.STRING_LENGTH)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.COMPARE)]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmedPassword { get; set; }

        [Required(ErrorMessage = ConstantHelpers.MESSAGES.VALIDATION.REQUIRED)]
        [Display(Name = "Roles")]
        public List<string> SelectedRoles { get; set; }

        [Display(Name = "Dependencias")]
        public List<Guid> SelectedDependencies { get; set; }
    }
}
